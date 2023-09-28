using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Models
{
    public class HomeViewModel
    {
        public IEnumerable<WateringSchedule> WateringSchedules { get; set; }
        public IEnumerable<Pot> Pots { get; set; }
        public short PotCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:d-MMM h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Sunrise { get; set; }
        [DisplayFormat(DataFormatString = "{0:d-MMM h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Sunset { get; set; }
        public bool FlgMorningDrink { get; set; }
        public bool FlgEveningDrink { get; set; }
        public string GrowName { get; set; }
        public string RecipeName { get; set; }
        public string CropName { get; set; }
        public short Day { get; set; }
        public short Week { get; set; }
        public string WeatherText { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal GrowRoomTemp { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal GrowRoomHumidity { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ReservoirTemp { get; set; }

        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal ReservoirVolume { get; set; }
        public short DaylightHours { get; set; }
        
        public short FertEvents { get; set; }
        public short MinsPerFert { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM d}")]
        public DateTime StartDate { get; set; }

    }
}
