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


namespace KTR.ViewModels
{
    public class RecipeViewModel
    {

        public int RecipeId { get; set; }

        [Required]
        [Display(Name = "Recipe Name")]
        public string Rname { get; set; }

      [Required]
        public int UserId { get; set; }

       [Required]
        public int Servings { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> CatName { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public IEnumerable<SelectListItem> StatusName {get; set;}
       
        public DateTime LastUpdated { get; set; }

        [Required]
        [Display(Name = "Main Ingredient")]
        public int? MainId { get; set; }
        public IEnumerable<SelectListItem> MainName { get; set; }

        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string RegId { get; set; }

        [Required]
        [Display(Name = "Submitted By")]
        public Users User { get; set; }
        public IEnumerable<SelectListItem> DisplayName;


        public IEnumerable<RecipeCategory> recipeCategory;
        public IEnumerable<MainIngredient> mainIngredient;
        public IEnumerable<RecipeStatus> recipeStatus;
        public IEnumerable<Users> displayName;
        public IEnumerable<Recipes> Recipe_Id;
    }
}
