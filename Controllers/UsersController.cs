using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using KTR.Models;
using KTR.ViewModels;

namespace KTR.Controllers
{
    public class UsersController : Controller
    {
        private readonly KTRContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(KTRContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // *********************************************************************************************************
        //                                          GET: Users
        // *********************************************************************************************************
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // *********************************************************************************************************
        //                                      GET: UserProfile/Create User Profile
        // *********************************************************************************************************
        [Authorize]
        public IActionResult CreateProfile()
        {
            return View();
        }


        // *********************************************************************************************************
        //                                          GET: Users/Create User Login
        // *********************************************************************************************************
        public IActionResult CreateUser()
        {
            return View();
        }
        // *********************************************************************************************************
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // *********************************************************************************************************
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("UserId,Fname,Lname,Email,DisplayName,RegId")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Recipes");
            }
            return View(users);
        }

        // *********************************************************************************************************
        // POST: Users/CreateProfule
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // *********************************************************************************************************
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProfile([Bind("UserId,Fname,Lname,Email,DisplayName,RegId")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Recipes");
                //return RedirectToAction(nameof(Recipes));
            }
            return View(users);
        }

        // *********************************************************************************************************
        //                                              GET: Users/Edit/5
        // *********************************************************************************************************
        //[Authorize]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _context.Users.FindAsync(id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(users);
        //}



        // *********************************************************************************************************
        //                                              GET: Users/Edit/5   TEST
        // *********************************************************************************************************

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var CurrentUserId = _userManager.GetUserId(User);
            var CurrentUserName = _userManager.GetUserName(User);

            if (CurrentUserId == "URegId")
            {

                //action goes here
                 if (id == null)
                 {
                        return NotFound();
                  }

                var users = await _context.Users.FindAsync(id);
                    if (users == null)
                     {
                     return NotFound();
                     }

                return View(users);
            }
            if (CurrentUserName == "admin@ktr.com")
                {
                    //same action here
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var users = await _context.Users.FindAsync(id);
                    if (users == null)
                    {
                        return NotFound();
                    }

                    return View(users);
              }

            return NotFound();



        }





        // *********************************************************************************************************
        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // *********************************************************************************************************
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Fname,Lname,Email,DisplayName")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // *********************************************************************************************************
        //                                                  GET: Users/Delete/5
        // *********************************************************************************************************

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // *********************************************************************************************************
        //                                      POST: Users/Delete/5
        // *********************************************************************************************************
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
