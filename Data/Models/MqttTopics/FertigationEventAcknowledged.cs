using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public static class FertigationEventAcknowledged
    {
        [JsonProperty("CommandID")]
        public static Guid CommandID { get; set; }

        [JsonProperty("FE_ebbFlowmeter_ACK")]
        public static bool FlgFE_ebbFlowmeter_ACK { get; set; }

        [JsonProperty("FE_ebbPump_ACK")]
        public static bool FlgFE_ebbPump_ACK { get; set; }

        [JsonProperty("FE_flowFlowmeter_ACK")]
        public static bool FlgFE_flowFlowmeter_ACK { get; set; }

        [JsonProperty("FE_flowPump_ACK")]
        public static bool FlgFE_flowPump_ACK { get; set; }

        [JsonProperty("FE_potOverflow_ACK")]
        public static bool FlgFE_potOverflow_ACK { get; set; }

        [JsonProperty("FE_ebbSolenoids_ACK")]
        public static bool FlgFE_ebbSolenoids_ACK { get; set; }

        [JsonProperty("FE_flowSolenoids_ACK")]
        public static bool FlgFE_flowSolenoids_ACK { get; set; }


        public static bool FlgAllAcknowledged
        {
            get
            {
                return FlgFE_ebbFlowmeter_ACK &&
                       FlgFE_ebbPump_ACK &&
                       FlgFE_flowFlowmeter_ACK &&
                       FlgFE_flowPump_ACK &&
                       FlgFE_potOverflow_ACK &&
                       FlgFE_ebbSolenoids_ACK && 
                       FlgFE_flowSolenoids_ACK;
            }
        }

        public static void ResetFlags()
        {
            CommandID = Guid.Empty;
            FlgFE_ebbFlowmeter_ACK = false;
            FlgFE_ebbPump_ACK = false;
            FlgFE_flowFlowmeter_ACK = false;
            FlgFE_flowPump_ACK = false;
            FlgFE_potOverflow_ACK = false;
            FlgFE_ebbSolenoids_ACK = false;
            FlgFE_flowSolenoids_ACK = false;
        }
    }
}
