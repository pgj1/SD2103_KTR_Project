using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using KTR.Models;
using KTR.ViewModels;


namespace KTR.Controllers
{
    public class RegisterAccountController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private KTRContext _context;
        private readonly IHostingEnvironment _webroot;

        public RegisterAccountController(KTRContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            
        }


        //Create username and password 
        public IActionResult CreateUser()
        {
            return View();
            //return RedirectToAction("Index", "Recipes");

        }




        //Create account profile
        public IActionResult CreateProfile()
        {
            return View();
            //return RedirectToAction("Index", "Recipes");

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
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile([Bind("FName,LName,DisplayName,Email,RegId")] Users user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Recipes));
            }
            return View();
        }

    }

   
}


