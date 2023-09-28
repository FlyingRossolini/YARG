using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class FertigationEventAcknowledged
    {
        public bool FlgFE_ebbFlowMeter_ACK { get; set; }
        public bool FlgFE_ebbPump_ACK { get; set; }
        public bool FlgFE_flowFlowMeter_ACK { get; set; }
        public bool FlgFE_flowPump_ACK { get; set; }
        public bool FlgFE_potOverflow_ACK { get; set; }
        public bool FlgFE_ebbSolenoids_ACK { get; set; }
        public bool FlgFE_flowSolenoids_ACK { get; set; }


        public bool FlgAllAcknowledged
        {
            get
            {
                return FlgFE_ebbFlowMeter_ACK &&
                       FlgFE_ebbPump_ACK &&
                       FlgFE_flowFlowMeter_ACK &&
                       FlgFE_flowPump_ACK &&
                       FlgFE_potOverflow_ACK &&
                       FlgFE_ebbSolenoids_ACK && 
                       FlgFE_flowSolenoids_ACK;
            }
        }
    }
}
