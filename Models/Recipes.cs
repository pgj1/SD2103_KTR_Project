using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
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
       [DisplayName ("Recipe Name")]
        public string RecipeName { get; set; }

        [Required]
        [DisplayName ("User Id")]
        public int UserId { get; set; }

        [Required]
        public int Servings { get; set; }

        [Required]
        [DisplayName ("Food Category")]
        public int CategoryId { get; set; }

        [Required]
        [DisplayName ("Visibility")]
        public int StatusId { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [DisplayName ("Main Ingredient")]
        public int? MainId { get; set; }

        public string Description { get; set; }
             
        [DisplayName ("Image")]
        public string PhotoPath { get; set; }

        [Required]
        public string RegId { get; set; }

        public RecipeCategory CatName { get; set; }

        public MainIngredient MainName { get; set; }

        public RecipeStatus StatusName { get; set; }

        public Users User { get; set; }

       // public Users DisplayName { get; set; }

        public ICollection<Ingredients> Ingredients { get; set; }

        public ICollection<Preparation> Preparation { get; set; }

       // public IEnumerable<Recipes> RecipeEnum { get; set; }

       
       
    }
}
