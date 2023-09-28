using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class LightCycle
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [Range(12,18)]
        [Display(Name = "Hours of Daylight per Day")]
        public byte DaylightHours { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}
