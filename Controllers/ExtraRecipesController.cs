using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KTR.Models;

namespace KTR.Controllers
{
    public class ExtraRecipesController : Controller
    {
        private readonly KTRContext _context;

        public ExtraRecipesController(KTRContext context)
        {
            _context = context;
        }

        // GET: ExtraRecipes
        public async Task<IActionResult> Index()
        {
            var kTRContext = _context.Recipes.Include(r => r.CatName).Include(r => r.MainName).Include(r => r.StatusName).Include(r => r.User);
            return View(await kTRContext.ToListAsync());
        }

        // GET: ExtraRecipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
                .Include(r => r.CatName)
                .Include(r => r.MainName)
                .Include(r => r.StatusName)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipes == null)
            {
                return NotFound();
            }

            return View(recipes);
        }

        // GET: ExtraRecipes/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
            ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName");
            ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: ExtraRecipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,RecipeName,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,Description,PhotoPath,RegId")] Recipes recipes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
            ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
            ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipes.UserId);
            return View(recipes);
        }

        // GET: ExtraRecipes/Edit/5
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
            ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
            ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
            ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipes.UserId);
            return View(recipes);
        }

        // POST: ExtraRecipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,RecipeName,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,Description,PhotoPath,RegId")] Recipes recipes)
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
            ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
            ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
            ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipes.UserId);
            return View(recipes);
        }

        // GET: ExtraRecipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
                .Include(r => r.CatName)
                .Include(r => r.MainName)
                .Include(r => r.StatusName)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipes == null)
            {
                return NotFound();
            }

            return View(recipes);
        }

        // POST: ExtraRecipes/Delete/5
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
