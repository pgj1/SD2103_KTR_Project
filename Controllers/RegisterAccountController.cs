using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;

namespace KTR.Controllers
{
    public class RegisterAccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterAccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Create account profile
        public IActionResult CreateProfile()
        {
            //return View();
            return RedirectToAction("Index", "Recipes");

        }
        /////////////////Create Username and password //////////////////////////////////////
        [HttpPost]
        public IActionResult CreateUser(string username, string password)
        {
            IdentityUser newUser = new IdentityUser();
            newUser.UserName = username;
            newUser.Email = username;

            IdentityResult result = _userManager.CreateAsync(newUser, password).Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(newUser, "RegisteredUser").Wait();

                _signInManager.SignInAsync(newUser, false).Wait();
            }

            return RedirectToAction("CreateProfile", "Users");
        }


    }
}
