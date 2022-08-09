using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class RecipeStep
    {
        [Key]
        public Guid ID { get; set; }

        public Guid RecipeID { get; set; }

        public short WeekNumber { get; set; }

        public Guid LightCycleID { get; set; }

        public string LightCycleName { get; set; }

        [Required]
        [Range(1, 10)]
        [Display(Name = "Irrigations / Day")]
        public short IrrigationEventsPerDay { get; set; }

        [Required]
        [Display(Name = "Soak Time")]
        public short SoakTime { get; set; }
        
        [Required]
        [Display(Name = "Morning sip?")]
        public bool IsMorningSip { get; set; }

        [Required]
        [Display(Name = "Evening sip?")]
        public bool IsEveningSip { get; set; }

        public IEnumerable<RecipeStepLimit> RecipeStepLimits { get; set; }

        public IEnumerable<RecipeStepAmount> RecipeStepAmounts { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }

    }
}
