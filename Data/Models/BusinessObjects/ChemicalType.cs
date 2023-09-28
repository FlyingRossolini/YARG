using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class ChemicalType
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Display(Name="H2O2?")]
        public bool IsH2O2 { get; set; }
        [Display(Name = "pH Up?")]
        public bool IsPhUp { get; set; }
        [Display(Name = "pH Down?")]
        public bool IsPhDown { get; set; }
        public short Sorting { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}
