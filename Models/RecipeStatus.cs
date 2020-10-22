using System;
using System.Collections.Generic;

namespace KTR.Models
{
    public partial class RecipeStatus
    {
        public RecipeStatus()
        {
            Recipes = new HashSet<Recipes>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}
