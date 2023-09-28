using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class RPIHeartbeatTopic
    {
        public Guid ID { get; set; }

        [JsonProperty("MACAddress")]
        public string MacAddress { get; set; }

        [JsonProperty("Hostname")]
        public string Hostname { get; set; }

        [JsonProperty("CPUUsage")]
        public float CpuUsage { get; set; }

        [JsonProperty("MemoryUsage")]
        public float MemoryUsage { get; set; }

        [JsonProperty("DiskUsage")]
        public float DiskUsage { get; set; }

        [JsonProperty("Uptime")]
        public uint Uptime { get; set; }

        [JsonProperty("Temperature")]
        public string Temperature { get; set; }

        [JsonProperty("LoadAverage")]
        public float LoadAverage { get; set; }

        [JsonProperty("Voltage")]
        public float Voltage { get; set; }

        [JsonProperty("YARGAppCurrentTasks")]
        public short YargAppCurrentTasks { get; set; }

        [JsonProperty("YARGAppTaskLimit")]
        public short YargAppTaskLimit { get; set; }

        [JsonProperty("YARGAppCPUCount")]
        public string YargAppCpuCount { get; set; }

        [JsonProperty("YARGAppStatus")]
        public string YargAppStatus { get; set; }

        [JsonProperty("lastBackupEndTime")]
        public DateTime LastBackupEndTime { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
