using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace KTR.Models
{
    public partial class Ingredients
    {
        public int IngredientId { get; set; }
        
        [Required]
        public string Amt { get; set; }
        
        [Required]
        public string Unit { get; set; }
        public string Item { get; set; }
        public string Prep { get; set; }

        [Required]
        public int RecipeId { get; set; }
        public DateTime LastUpdated { get; set; }
        public string IRegId { get; set; }

        public AspNetUsers IReg { get; set; }
        //public Recipes RecipeName { get; set; }

        public Recipes Recipe { get; set; }
       // public Users Users { get; set; }



    }
}
