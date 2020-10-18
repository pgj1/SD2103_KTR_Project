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
    public class RecipesController : Controller
    {
        private readonly KTRContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _webroot;

        public RecipesController(KTRContext context, UserManager<IdentityUser> userManager, IHostingEnvironment webroot)
        {
            _context = context;
            _userManager = userManager;
            _webroot = webroot;
        }

        // GET: Recipes
       
        public async Task<IActionResult> Index()
        {
              ///       var kTRContext = _context.Recipes.Include(r => r.CategoryId).Include(r => r.MainId).Include(r => r.StatusId).Include(r => r.UserId).Include(r => r.Description).Include(r => r.Servings).Include(r => r.PhotoPath).Include(m => m.RecipeId);
              //       return View(await kTRContext.ToListAsync());


            return View(await _context.Recipes.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
            //    .Include(r => r.CategoryId)
            //    .Include(r => r.MainId)
            //    .Include(r => r.StatusId)
            //    .Include(r => r.Description)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipes == null)
            {
                return NotFound();
            }

            return View(recipes);
        }

        // GET: Recipes/Create
       // [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Recipes, "CategoryId","CatName");
            ViewBag.MainId = new SelectList(_context.Recipes, "MainId", "MainName");
            ViewBag.StatusId = new SelectList(_context.Recipes, "StatusId", "StatusName");
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,Rname,Description,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId")] Recipes recipes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Recipes, "CategoryId", "CatName", recipes.CategoryId);
            ViewBag.MainId = new SelectList(_context.Recipes, "MainId", "MainName", recipes.MainId);
            ViewBag.StatusId = new SelectList(_context.Recipes, "StatusId", "StatusName", recipes.StatusId);
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Email", recipes.UserId);
            return View(recipes);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes.FindAsync(id);
            if (recipes == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Recipes, "CategoryId", "CatName", recipes.CategoryId);
            ViewData["MainId"] = new SelectList(_context.Recipes, "MainId", "MainName", recipes.MainId);
            ViewData["StatusId"] = new SelectList(_context.Recipes, "StatusId", "StatusName", recipes.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipes.UserId);
            return View(recipes);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,Rname,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,Description")] Recipes recipes)
        {
            if (id != recipes.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipesExists(recipes.RecipeId))
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
            ViewData["CategoryId"] = new SelectList(_context.Recipes, "CategoryId", "CatName", recipes.CategoryId);
            ViewData["MainId"] = new SelectList(_context.Recipes, "MainId", "MainName", recipes.MainId);
            ViewData["StatusId"] = new SelectList(_context.Recipes, "StatusId", "StatusName", recipes.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipes.UserId);
            return View(recipes);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
                .Include(r => r.CategoryId)
                .Include(r => r.MainId)
                .Include(r => r.StatusId)
                .Include(r => r.UserId)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipes == null)
            {
                return NotFound();
            }

            return View(recipes);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipes = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipesExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }
    }
}
