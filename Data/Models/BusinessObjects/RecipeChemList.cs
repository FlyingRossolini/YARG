using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class RecipeChemList
    {
        [Key]
        public Guid ID { get; set; }

        public Guid RecipeID { get; set; }

        public Guid ChemicalID { get; set; }
        public string ChemicalName { get; set; }

        [Required]
        [Range(1,5)]
        public short Mixtime { get; set; }

        //[Required]
        //[Range(1,6)]
        //public short Priority { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        
    }
}
