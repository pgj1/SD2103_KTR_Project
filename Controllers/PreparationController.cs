using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using KTR.Models;
using KTR.ViewModels;
using System.IO;

namespace KTR.Controllers
{
    public class PreparationController : Controller
    {
        private readonly KTRContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PreparationController(KTRContext context, UserManager<IdentityUser> userManager, IHostingEnvironment webroot)
        {
            _context = context;
            _userManager = userManager;

        }

        // **********************************************************************************
        //                            GET: Preparation Index()
        //                                  For Admin
        // **********************************************************************************

        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Index()
        {
            var kTRContext = _context.Preparation.Include(p => p.Recipe);
            return View(await kTRContext.ToListAsync());
        }


        // **********************************************************************************
        //                      GET: Preparation MyPrep()
        //                              For users
        // **********************************************************************************

        [Authorize]
        public async Task<IActionResult> MyPrep(int id)
        {
            ViewBag.SavedRecipeId = id;
            string ShowId = _userManager.GetUserId(User);
            var kTRContext = _context.Preparation.Include(p => p.Recipe);


            if (ShowId != null)
            {

                var PrepList = (from p in _context.Preparation
                                  where p.RecipeId == id
                                  orderby p.Step
                                  select p);
                ViewBag.PrepList = PrepList;

                return View(await PrepList.ToListAsync());

            }

            return RedirectToAction("Home");
        }


       


        // GET: Preparation/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SavedRecipeId = id;

            if (id == null)
            {
                return NotFound();
            }

            var preparation = await _context.Preparation
                .Include(p => p.PReg)
                .Include(p => p.Recipe)
                .FirstOrDefaultAsync(m => m.PrepId == id);
            if (preparation == null)
            {
                return NotFound();
            }

            return View(preparation);
        }

        // ********************************************************************************
        // GET: Preparation/Create
        // ********************************************************************************
        [Authorize]
        public IActionResult Create(int id)
        {
            ViewBag.SavedRecipeId = id;
            ViewData["PRegId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeName");
            return View();
        }

        // POST: Preparation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("PrepId,Step,Instr,RecipeId,Updated,PRegId")] Preparation preparation)
        {
            ViewBag.SavedRecipeId = id;

            if (ModelState.IsValid)
            {
                _context.Add(preparation);
                await _context.SaveChangesAsync();
                return RedirectToAction(MyPrep(ViewBag.SavedRecipId));
            }
            ViewData["PRegId"] = new SelectList(_context.AspNetUsers, "Id", "Id", preparation.PRegId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeName", preparation.RecipeId);
            //return View(preparation);
            return RedirectToAction(MyPrep(ViewBag.SavedRecipId));
        }

        // GET: Preparation/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SavedRecipeId = id;

            if (id == null)
            {
                return NotFound();
            }

            var preparation = await _context.Preparation.FindAsync(id);
            if (preparation == null)
            {
                return NotFound();
            }
            ViewData["PRegId"] = new SelectList(_context.AspNetUsers, "Id", "Id", preparation.PRegId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeName", preparation.RecipeId);
            return View(preparation);
        }

        // POST: Preparation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrepId,Step,Instr,RecipeId,Updated,PRegId")] Preparation preparation)
        {
            ViewBag.SavedRecipeId = id;

            if (id != preparation.PrepId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(preparation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreparationExists(preparation.PrepId))
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
            ViewData["PRegId"] = new SelectList(_context.AspNetUsers, "Id", "Id", preparation.PRegId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeName", preparation.RecipeId);
            return View(preparation);
        }

        // GET: Preparation/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.SavedRecipeId = id;

            if (id == null)
            {
                return NotFound();
            }

            var preparation = await _context.Preparation
               .FirstOrDefaultAsync(m => m.PrepId == id);

            //var preparation = await _context.Preparation
            //    .Include(p => p.PReg)
            //    .Include(p => p.Recipe)
            //    .FirstOrDefaultAsync(m => m.PrepId == id);
            if (preparation == null)
            {
                return NotFound();
            }

            return View(preparation);
        }

        // POST: Preparation/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var preparation = await _context.Preparation.FindAsync(id);
            _context.Preparation.Remove(preparation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreparationExists(int id)
        {
            return _context.Preparation.Any(e => e.PrepId == id);
        }
    }
}
