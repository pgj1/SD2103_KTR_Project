using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
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
    public class IngredientsController : Controller
    {
        private readonly KTRContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _webroot;

        public IngredientsController(KTRContext context, UserManager<IdentityUser> userManager, IHostingEnvironment webroot)
        {
            _context = context;
            _userManager = userManager;
            _webroot = webroot;
        }


        // *********************************************************************************************************
        //                                         GET: Ingredients Index()
        // *********************************************************************************************************
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var kTRContext = _context.Ingredients.Include(i => i.Recipe);
            return View(await kTRContext.ToListAsync());
        }

        // *********************************************************************************************************
        //                                          GET: Ingredients/Show/5
        // *********************************************************************************************************
        public async Task<IActionResult> Show(int? iid)
        {
            ViewBag.SavedIngredientId = iid;

            if (iid == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.IngredientId == iid);
            if (ingredients == null)
            {
                return NotFound();
            }

            return View("IngredientsController","ShowIngredients");
        }

        // *********************************************************************************************************
        //                                          GET: Ingredients/Details/5
        // *********************************************************************************************************

        [Authorize]
        public async Task<IActionResult> Details(int? iid)
        {
            ViewBag.SavedIngredientId = iid;

            if (iid == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.IngredientId == iid);
            if (ingredients == null)
            {
                return NotFound();
            }

            return View(ingredients);
        }

        // *********************************************************************************************************
        //                                              GET: Ingredients/Create
        // *********************************************************************************************************

        public IActionResult Create(int? id)
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id");
            ViewData["RecipeName"] = (_context.Recipes, "RecipeId", "RecipeName");
            ViewBag.SavedRecipeId = id;
            

            return View();
        }


        // POST: Ingredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("IngredientId,Amt,Unit,Item,Prep,RecipeId,LastUpdated,IRegId,UserId")] Ingredients ingredients)
        {
             ViewBag.SavedRecipeId = id;

            if (ModelState.IsValid)
            {
                _context.Add(ingredients);
                await _context.SaveChangesAsync();
                //return RedirectToAction("ShowRecipes", "Recipes");
                return View();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeName", ingredients.RecipeId);
           // ViewBag.SavedRecipeId = ingredients.RecipeId;
            return View(ingredients);
        }

        // *********************************************************************************************************
        //                                          GET: Ingredients/Edit/5
        // *********************************************************************************************************

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SavedIngredientId = id;

            if (id == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients.FindAsync(id);
            if (ingredients == null)
            {
                return NotFound();
            }

            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeName", ingredients.RecipeId);
            ViewBag.SavedRecipeId = ingredients.RecipeId;
            
            return View(ingredients);
        }

        // *********************************************************************************************************
                                                       // POST: Ingredients/Edit/5
        // *********************************************************************************************************
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngredientId,Amt,Unit,Item,Prep,RecipeId,LastUpdated,IRegId,UserId")] Ingredients ingredients)
        {
            ViewBag.SavedIngredientId = id;

            if (id != ingredients.IngredientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientsExists(ingredients.IngredientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("ShowIngredients","ingredients.RecipeId");

            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeName", ingredients.RecipeId);
            return View(ingredients);

        }

        // *********************************************************************************************************
        //                                          GET: Ingredients/Delete/5
        // *********************************************************************************************************

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.SavedIngredientId = id;

            if (id == null)
            {
                return NotFound();
            }

            var ingredients = await _context.Ingredients
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.IngredientId == id);

            ViewBag.SavedRecipeId = ingredients.RecipeId;

            if (ingredients == null)
            {
                return NotFound();
            }

            return View(ingredients);
        }

        // *********************************************************************************************************
        //                                              POST: Ingredients/Delete/5
        // *********************************************************************************************************

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredients = await _context.Ingredients.FindAsync(id);
            _context.Ingredients.Remove(ingredients);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowIngredients","ViewBag.SavedRecipeId");
        }

        private bool IngredientsExists(int id)
        {
            return _context.Ingredients.Any(e => e.IngredientId == id);
        }
        // *********************************************************************************************************
        //                                          ShowIngredients()
        //                          Show Users their own ingredients so they can edit them
        // *********************************************************************************************************

        //public async Task<IActionResult> ShowIngredients(int id)
        //{
        //    KTRContext context = new KTRContext();
        //    string ShowId = _userManager.GetUserId(User);
        //    var RList = _context.Recipes;
        //    var IList = _context.Ingredients;
        //    var kTRContext = _context.Ingredients.Include(i => i.Recipe);


        //    if (id != 0)
        //    {

        //        var recipeList = (from r in context.Recipes
        //                          where r.RecipeId == id
        //                          select r.RecipeName);
        //        ViewBag.RecipeName = recipeList;

        //        Ingredients profile = await _context.Ingredients.FindAsync(id);

        //        if (profile != null)
        //        {
        //            var MyIList = (from i in context.Ingredients
        //                           where i.RecipeId == id
        //                           select i);
        //            ViewBag.IngredientList = MyIList;

        //            return View(await kTRContext.ToListAsync());
        //        }
        //        else
        //        {
        //            return RedirectToAction("Recipes", "ShowRecipes");
        //        }

        //    }
        //    return RedirectToAction("Recipes", "ShowRecipes");
        //}

        [Authorize]
        public async Task<IActionResult> ShowIngredients(int id)
        {
            ViewBag.SavedRecipeId = id;

            if (id != 0)
            {
                var kTRContext = _context.Ingredients.Include(i => i.Recipe);

                var MyIList = (from i in _context.Ingredients
                                   where i.RecipeId == id
                                   select i);
                    ViewBag.IngredientList = MyIList;

        //                                       return View(await kTRContext.ToListAsync());




                return View(await kTRContext.ToListAsync());
             }

            return RedirectToAction("Recipes", "ShowRecipes");

        }


    }
}
