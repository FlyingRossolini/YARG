using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class Chemical
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "Price Per L")]
        public decimal PricePerL { get; set; }

        [Required]
        [Range(1,100)]
        [Display(Name = "Mixing Priority")]
        public short MixPriority { get; set; }

        [Required]
        [Range(1,20)]
        [Display(Name = "Mixing Time (mins)")]
        public short MixTime { get; set; }

        [Required]
        public Guid ChemicalTypeID { get; set; }
        [Display(Name = "Chemical Type")]
        public string ChemicalTypeName { get; set; }
        [Required]
        [Range(0, 100)]
        [Display(Name = "In Stock Amount")]
        public decimal InStockAmount { get; set; }
        [Required]
        [Range(1, 100)]
        [Display(Name = "Minimum Reorder Point")]
        public short MinReorderPoint { get; set; }
        public short Sorting { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

    }
}
