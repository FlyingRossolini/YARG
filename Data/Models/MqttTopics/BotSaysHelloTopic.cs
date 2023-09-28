using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class BotSaysHelloTopic
    {
        public Guid ID { get; set; }
        public uint UptimeMillis { get; set; }
        public string MacAddress { get; set; }
        public string Hostname { get; set; }
        public int CpuFreqMHz { get; set; }
        public uint FlashChipSize { get; set; }
        public uint FlashChipSpeed { get; set; }
        public uint FreeFlash { get; set; }
        public float VccVoltage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
