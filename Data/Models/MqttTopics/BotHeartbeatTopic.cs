using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class BotHeartbeatTopic
    {
        public Guid ID { get; set; }
        public string Hostname { get; set; }
        public uint UptimeMillis { get; set; }
        public string Task { get; set; }
        public uint StackHighWaterMark { get; set; }
        public float VccVoltage { get; set; }
        public int RSSI { get; set; }
        public uint FreeHeap { get; set; }
        public uint HeapSize { get; set; }
        public float Temperature { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
