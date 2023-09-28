using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YARG.Models
{
    public class Recipe
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public IEnumerable<RecipeChemList> RecipeChems { get; set; }
        public IEnumerable<RecipeStep> RecipeSteps { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        
    }
}
