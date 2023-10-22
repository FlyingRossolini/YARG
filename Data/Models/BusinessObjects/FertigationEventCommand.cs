using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Models
{
    public class FertigationEventCommand
    {
        public Guid CommandID { get; set; }
        public Guid PotID { get; set; }
        public byte PotNumber { get; set; }
        public byte EbbSpeed { get; set; }
        public decimal EbbAmount { get; set; }
        public short EbbAntiShockRamp { get; set; }
        public decimal EbbExpectedFlowRate { get; set; }
        public byte EbbPumpErrorThreshold { get; set; } // 5% to 95%
        public short EbbPulsesPerLiter { get; set; }
        public int SoakDuration { get; set; }
        public byte FlowSpeed { get; set; }
        public short FlowAntiShockRamp { get; set; }
        public decimal FlowExpectedFlowRate { get; set; }
        public short FlowPumpErrorThreshold { get; set; }
        public short FlowPulsesPerLiter { get; set; }
    }
}
