using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace GardenMVC.DAL
{
    public class ConnectionStringManager
    {
        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json",optional:true,reloadOnChange:true);
            return builder.Build().GetSection("ConnectionStrings").GetSection("GardenConnection").Value;
        }
        public string GetExecuteTaskServiceCallSchedulingStatusString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build().GetSection("QuartzStrings").GetSection("ExecuteTaskServiceCallSchedulingStatus").Value;
        }
        public string GetMQTTServerIP()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build().GetSection("MQTTStrings").GetSection("MQTTConnectionIP").Value;
        }
        public string GetMQTTPort()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build().GetSection("MQTTStrings").GetSection("MQTTPort").Value;
        }
        public string GetMQTTUsername()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build().GetSection("MQTTStrings").GetSection("MQTTUsername").Value;
        }
        public string GetMQTTPassword()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build().GetSection("MQTTStrings").GetSection("MQTTPassword").Value;
        }


    }
}
