using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class RPIHelloTopic
    {
        public Guid ID { get; set; }

        [JsonProperty("MACAddress")]
        public string MACAddress { get; set; }

        [JsonProperty("Hostname")]
        public string Hostname { get; set; }

        [JsonProperty("OSName")]
        public string OSName { get; set; }

        [JsonProperty("OSVersion")]
        public string OSVersion { get; set; }

        [JsonProperty("BootloaderFirmwareVersion")]
        public string BootloaderFirmwareVersion { get; set; }

        [JsonProperty("CPUModel")]
        public string CPUModel { get; set; }

        [JsonProperty("CPUCores")]
        public int CPUCores { get; set; }

        [JsonProperty("CPUTemperature")]
        public float CPUTemperature { get; set; }

        [JsonProperty("CPUSerialNumber")]
        public string CPUSerialNumber { get; set; }

        [JsonProperty("TotalRAM")]
        public long TotalRAM { get; set; }

        [JsonProperty("TotalDiskSpace")]
        public long TotalDiskSpace { get; set; }

        [JsonProperty("TotalUsedDiskSpace")]
        public long TotalUsedDiskSpace { get; set; }

        [JsonProperty("UptimeMillis")]
        public long UptimeMillis { get; set; }

        [JsonProperty("FirstBootDateTime")]
        public DateTime FirstBootDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
