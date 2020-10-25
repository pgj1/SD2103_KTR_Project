using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace KTR.Models
{
    public partial class Recipes
    {
        public Recipes()
        {
            Ingredients = new HashSet<Ingredients>();
            Preparation = new HashSet<Preparation>();
            
            
        }
        
        public int RecipeId { get; set; }

       [Required]
        public string RecipeName { get; set; }

        public int UserId { get; set; }

        public int Servings { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public DateTime LastUpdated { get; set; }

        public int? MainId { get; set; }

        public string Description { get; set; }

        public string PhotoPath { get; set; }

        [Required]
        public string RegId { get; set; }

        public RecipeCategory CatName { get; set; }

        public MainIngredient Main { get; set; }

        public RecipeStatus StatusName { get; set; }

        public Users User { get; set; }

        public ICollection<Ingredients> Ingredients { get; set; }

        public ICollection<Preparation> Preparation { get; set; }
       
    }
}
