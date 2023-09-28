using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{

    public class Pot
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public short QueuePosition { get; set; }

        [Required]
        [Range(2, 13)]
        [Display(Name = "Soak Duration (Mins)")]
        public short EFDuration { get; set; }

        [Required]
        [Range(2, 20)]
        [Display(Name = "Capacity (Liters)")]
        public decimal EFAmount { get; set; }

        [Required]
        [Range(50, 100)]
        [Display(Name = "Ebb speed")]
        public short EbbSpeed { get; set; }

        [Required]
        [Range(50, 100)]
        [Display(Name = "Flow speed")]
        public short FlowSpeed { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string ChangedBy { get; set; }

        public DateTime ChangeDate { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}
