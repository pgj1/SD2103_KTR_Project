using System;
using System.Collections.Generic;

namespace KTR.Models
{
    public partial class MainIngredient
    {
        public MainIngredient()
        {
            Recipes = new HashSet<Recipes>();
        }

        public int MainId { get; set; }
        public string MainName { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}
