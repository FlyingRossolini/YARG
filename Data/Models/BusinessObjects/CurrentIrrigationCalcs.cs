using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Models
{
    public class CurrentIrrigationCalcs
    {
        public string GrowName { get; set; }
     
        public DateTime GrowStartDate { get; set; }
        public short GrowDay { get; set; }
        public short GrowWeek { get; set; }
        public bool IsDay { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public DateTime SunriseYesterday { get; set; }
        public DateTime SunsetYesterday { get; set; }
        public short IrrigationEventsPerDay { get; set; }
        public short SoakTime { get; set; }
        public bool IsMorningSip { get; set; }
        public bool IsEveningSip { get; set; }
        public short DaylightHoursPerDay { get; set; }
        public Guid RecipeID { get; set; }
        public string RecipeName { get; set; }
        public string CropName { get; set; }
        public Guid GrowSeasonID { get; set; }
        public decimal GrowRoomTemp { get; set; }
        public decimal GrowRoomHumidity { get; set; }
        public decimal ReservoirTemp { get; set; }
        public decimal ReservoirVolume { get; set; }
        public string WeatherText { get; set; }
    }
}
