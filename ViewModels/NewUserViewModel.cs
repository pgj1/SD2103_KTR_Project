using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using KTR.Models;

namespace KTR.ViewModels
{
    public class NewUserViewModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Lname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public string RegId { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Username { get; set; }



        //public IEnumerable<RecipeCategory> recipeCategory;
        //public IEnumerable<MainIngredient> mainIngredient;
        //public IEnumerable<RecipeStatus> recipeStatus;
        //public IEnumerable<Users> displayName;
        //public IEnumerable<Recipes> Recipe_Id;
        //public IEnumerable<Aspnetusers> email;




    }
}



