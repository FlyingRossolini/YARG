using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class GrowSeason
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Display(Name = "Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        [Required]
        [Display(Name = "Sunrise")]
        [DataType(DataType.Time)]
        public DateTime SunriseTime { get; set; }
        //public DateTime SunsetTime { get; set; }
        public Guid CropID { get; set; }
        public Guid RecipeID { get; set; }
        //[Required]
        //[Display(Name = "Morning drink?")]
        //public bool FlgAddMorningSplash { get; set; }
        //[Required]
        //[Display(Name = "Evening drink?")]
        //public bool FlgAddEveningSplash { get; set; }
        //[Required]
        //[Display(Name = "Times / Day")]
        //[Range(1, 10)]
        //public byte EFEventsPerDay { get; set; }
        [Required]
        [Display(Name = "Completed?")]
        public bool IsComplete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }
        [Display(Name = "Crop")]
        public string CropName { get; set; }
        [Display(Name = "Recipe")]
        public string RecipeName { get; set; }
        //[Display(Name="Light Cycle")]
        //public string LightCycleName { get; set; }

    }
}
