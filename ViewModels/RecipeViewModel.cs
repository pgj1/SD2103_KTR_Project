using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KTR.ViewModels;
using KTR.Models;
using System.ComponentModel;



namespace KTR.ViewModels
{
    public class RecipeViewModel
    {

        public Recipes RecipeId { get; set; }

        [Required]
        [Display(Name = "Recipe Name")]
        public Recipes RecipeName { get; set; }

        [Required]
        public Users UserId { get; set; }

        [Required]
        public Recipes Servings { get; set; }

        [Required]
        [Display(Name = "Food Category")]
        public RecipeCategory CategoryId { get; set; }


        public RecipeCategory CatName  { get; set; }


        public Ingredients Amt { get; set; }

        public Ingredients Unit { get; set; }

        public Ingredients Item { get; set; }

        public Ingredients Prep { get; set; }

        public Ingredients IRecipeId { get; set; }


        public Ingredients IRegId { get; set; }


       // public IEnumerable<SelectListItem> CatName { get; set; }


        [Required]
        [Display(Name = "Visibility")]
        public RecipeStatus StatusId { get; set; }

        public IEnumerable<SelectListItem> StatusName { get; set; }

        public DateTime LastUpdated { get; set; }

        [Required]
        [Display(Name = "Main Ingredient")]
        public MainIngredient MainId { get; set; }

        public IEnumerable<SelectListItem> MainName { get; set; }

        public Recipes Description { get; set; }

        public Recipes PhotoPath { get; set; }
        
        [Required]
        public Recipes RegId { get; set; }

       // [Required]
      //  [Display(Name = "Submitted By")]
       // public Users User { get; set; }
        public IEnumerable<SelectListItem> DisplayName;

                      
        public IEnumerable<RecipeCategory> recipeCategory;
        public IEnumerable<MainIngredient> mainIngredient;
        public IEnumerable<RecipeStatus> recipeStatus;
        public IEnumerable<Users> displayName;
        //public IEnumerable<Recipes> Recipe_Id;

        public IEnumerator<Recipes> model;

    }
}
