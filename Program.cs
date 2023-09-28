using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YARG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup().UseContentRoot(Directory.GetCurrentDirectory())
                    //.UseUrls("http://*:5000");
                    //webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel().UseContentRoot(Directory.GetCurrentDirectory())
                    .UseUrls("http://*:5000").UseStartup<Startup>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);
                }).ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                    config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false);
                    config.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: false);
                });
    }
}
