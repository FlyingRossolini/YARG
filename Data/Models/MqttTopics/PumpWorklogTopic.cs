using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class PumpWorklogTopic
    {
        public Guid ID { get; set; }

        [JsonProperty("pumpID")]
        public Guid PumpID { get; set; }

        [JsonProperty("runtimeMillis")]
        public long RuntimeMillis { get; set; }

        [JsonProperty("flowAmountPulses")]
        public long FlowAmountPulses { get; set; }

        public decimal FlowAmountmL { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
