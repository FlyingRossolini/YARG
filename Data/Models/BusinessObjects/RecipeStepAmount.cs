using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class RecipeStepAmount
    {
        [Key]
        public Guid ID { get; set; }

        public Guid RecipeStepID { get; set; }
        public Guid ChemicalID { get; set; }
        public string ChemicalName { get; set; }
        public Guid ChemicalTypeID { get; set; }
        public decimal Amount { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }

    }
}
