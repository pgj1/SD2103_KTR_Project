using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KTR.Models
{
    public partial class Ingredients
    {
        [Key]
        public int IngredientId { get; set; }
        public string Amt { get; set; }
        public string Unit { get; set; }
        public string Item { get; set; }
        public string Prep { get; set; }
        public int RecipeId { get; set; }
        public DateTime LastUpdated { get; set; }

        public Recipes Recipe { get; set; }
    }
}
