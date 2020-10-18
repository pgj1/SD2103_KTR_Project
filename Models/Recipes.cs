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
using Microsoft.Extensions.DependencyInjection;

namespace KTR.Models
{
    public partial class Recipes
    {
        [Key]
        public int RecipeId { get; set; }
        
        [Display(Name = "Recipe Name")]
        public string Rname { get; set; }
        
        public int UserId { get; set; }
       
        public int Servings { get; set; }


        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
     //   public IEnumerable<SelectListItem> CatName { get; set; }



      //  public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public DateTime LastUpdated { get; set; }
        public int? MainId { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string RegId { get; set; }

        public Users User { get; set; }
    }
}
