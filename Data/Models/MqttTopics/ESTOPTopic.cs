using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class ESTOPTopic
    {
        public DateTime ExpiryDate { get; set; }

        [JsonProperty("Hostname")]
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
