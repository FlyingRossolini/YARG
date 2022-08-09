using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Models
{
    public class MixingFanScheduleCommand
    {
        public short FanNumber { get; set; }
        public short PumpSpeed { get; set; }
        public short OverSpeed { get; set; }
        public short Duration { get; set; }
        public Guid MixingFanScheduleID { get; set; }

    }
}