using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class Jar
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [Range(1,9)]
        [Display(Name = "Mixing fan position")]
        public short MixFanPosition { get; set; }
        [Required]
        public Guid ChemicalID { get; set; }
        public string ChemicalName { get; set; }
        [Required]
        [Range(0, 24)]
        [Display(Name = "Mix times / day")]
        public short MixTimesPerDay { get; set; }
        [Required]
        [Range(0, 59)]
        [Display(Name = "Mix duration (mins)")]
        public short Duration { get; set; }
        [Required]
        [Range(1, 4)]
        [Display(Name = "Jar capacity (L)")]
        public decimal Capacity { get; set; }
        [Required]
        [Range(0, 4)]
        [Display(Name = "Current amount (L)")]
        public decimal CurrentAmount { get; set; }

        [Required]
        [Range(1000,5000)]
        [Display(Name = "Mixing fan overspeed")]
        public short MixFanOverSpeed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}
