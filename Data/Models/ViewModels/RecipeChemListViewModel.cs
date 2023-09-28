using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Models
{
    public class RecipeChemListViewModel
    {
        public Guid RecipeID { get; set; }

        public Guid ChemicalID { get; set; }

        public Guid ChemicalTypeID { get; set; }
        public string ChemicalTypeName { get; set; }

        public string ChemicalName { get; set; }

        public short MixPriority { get; set; }

        public short MixTime { get; set; }
    }
}
