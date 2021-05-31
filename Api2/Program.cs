using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;

namespace Api2
{
    public class Program
    {
        public const string ServiceName = "service2";
        public const string Version = "v1";
        
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls($"http://*:{ApplicationBuilderExtensions.Ip}:{ApplicationBuilderExtensions.Port}") //set url
                        .UseKestrel()
                        .UseStartup<Startup>();
                });




    }
}

