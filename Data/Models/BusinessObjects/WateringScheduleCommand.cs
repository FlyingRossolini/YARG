using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenMVC.Models
{
    public class WateringScheduleCommand
    {
        public short PotNumber { get; set; }
        public short EbbSpeed { get; set; }
        public short FlowSpeed { get; set; }
        public short Amount { get; set; }
        public short Duration { get; set; }
        public Guid WateringScheduleID { get; set; }

    }
}
