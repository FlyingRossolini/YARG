using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class GenericFE_ACK
    {
        public Guid CommandID { get; set; }

        [JsonProperty("FE_ebbFlowmeter_ACK")]
        public bool FlgFE_ebbFlowmeter_ACK { get; set; }

        [JsonProperty("FE_ebbPump_ACK")]
        public bool FlgFE_ebbPump_ACK { get; set; }

        [JsonProperty("FE_flowFlowmeter_ACK")]
        public bool FlgFE_flowFlowmeter_ACK { get; set; }

        [JsonProperty("FE_flowPump_ACK")]
        public bool FlgFE_flowPump_ACK { get; set; }

        [JsonProperty("FE_potOverflow_ACK")]
        public bool FlgFE_potOverflow_ACK { get; set; }

        [JsonProperty("FE_ebbSolenoids_ACK")]
        public bool FlgFE_ebbSolenoids_ACK { get; set; }

        [JsonProperty("FE_flowSolenoids_ACK")]
        public bool FlgFE_flowSolenoids_ACK { get; set; }

    }
}
