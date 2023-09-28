using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Data.Models.MqttTopics
{
    public class FertigationEventDone
    {
        public bool FlgFE_ebbFlowMeter_DN { get; set; }
        public bool FlgFE_ebbPump_DN { get; set; }
        public bool FlgFE_flowFlowMeter_DN { get; set; }
        public bool FlgFE_flowPump_DN { get; set; }
        public bool FlgFE_ebbSolenoids_DN { get; set; }
        public bool FlgFE_flowSolenoids_DN { get; set; }


        public bool FlgAllDone
        {
            get
            {
                return FlgFE_ebbFlowMeter_DN &&
                       FlgFE_ebbPump_DN &&
                       FlgFE_flowFlowMeter_DN &&
                       FlgFE_flowPump_DN &&
                       FlgFE_ebbSolenoids_DN &&
                       FlgFE_flowSolenoids_DN;
            }
        }
    }
}
