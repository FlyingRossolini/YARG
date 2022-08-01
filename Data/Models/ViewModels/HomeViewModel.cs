using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GardenMVC.Models
{
    public class HomeViewModel
    {
        public IEnumerable<WateringSchedule> wateringSchedules { get; set; }
        public IEnumerable<Pot> pots { get; set; }
        public short potCount { get; set; }
        public DateTime sunrise { get; set; }
        public DateTime sunset { get; set; }
        public bool flgMorningDrink { get; set; }
        public bool flgEveningDrink { get; set; }
        public string growName { get; set; }
    }
}
