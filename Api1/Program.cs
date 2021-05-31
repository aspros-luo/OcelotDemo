using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api1
{
    public class Program
    {
        public const string ServiceName = "service1";
        public const string Version = "v1";
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
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
