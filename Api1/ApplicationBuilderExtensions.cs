using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Api1
{
    public static class ApplicationBuilderExtensions
    {
        public static readonly string Ip = GetLocalIpAddress();
        public static readonly int Port = GetNextAvailablePort();

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IApplicationLifetime lifetime,Uri url)
        {
            //var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{Program.IP}:8500"));//如果服务和 Consul 在同一台服务器上，使用此代码
            var consulClient = new ConsulClient(x => x.Address = url);//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{Ip}:{Port}/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = Program.ServiceName,
                Address = Ip,
                Port = Port,
                Tags = new[] { $"urlprefix-/{Program.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });
            return app;
        }

        public static string GetLocalIpAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    //                    if (!address.IsDnsEligible)
                    //                    {
                    //                        if (mostSuitableIp == null)
                    //                            mostSuitableIp = address;
                    //                        continue;
                    //                    }
                    //
                    //                    // The best IP is the IP got from DHCP server
                    //                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    //                    {
                    //                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                    //                            mostSuitableIp = address;
                    //                        continue;
                    //                    }

                    return address.Address.ToString();
                }
            }

            return "";
        }

        public static int GetNextAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            int port;
            listener.Start();
            port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            listener.Server.Dispose();
            return port;
        }
    }
}
