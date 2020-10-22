using System;
using System.Collections.Generic;

namespace KTR.Models
{
    public partial class RecipeCategory
    {
        public RecipeCategory()
        {
            Recipes = new HashSet<Recipes>();
        }

        public int CategoryId { get; set; }
        public string CatName { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}
