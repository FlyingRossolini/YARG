using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Models
{
    public class RecipeViewModel
    {
        public IEnumerable<Recipe> recipes { get; set; }

        public IEnumerable<Location> locations { get; set; }

        public IEnumerable<RecipeChemListViewModel> recipeChemListViewModels { get; set; }
    }
}
