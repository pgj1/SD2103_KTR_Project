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
using System.Collections;
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
        
        private readonly IEnumerable<Recipes> recipeEnum;


        public RecipesController(KTRContext context, UserManager<IdentityUser> userManager, IHostingEnvironment webroot)
        {
            _context = context;
            _userManager = userManager;
            _webroot = webroot;
            
            //_recipeList = recipeList;
        }

        // **************************** GET: Recipes  Index()  *******************************

        public async Task<IActionResult> Index()
        {
            ///       var kTRContext = _context.Recipes.Include(r => r.CategoryId).Include(r => r.MainId).Include(r => r.StatusId).Include(r => r.UserId).Include(r => r.Description).Include(r => r.Servings).Include(r => r.PhotoPath).Include(m => m.RecipeId);
            //       return View(await kTRContext.ToListAsync());

            //ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
            ViewBag.CategoryId = (_context.RecipeCategory, "CategoryId", "CatName");
            //ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName");
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainName");
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "DisplayName");
            //ViewBag.UserId = new SelectList(_context.Users, "DisplayName");


            //var kTRContext = _context.Ingredients.Include(i => i.Recipe);
            //return View(await kTRContext.ToListAsync());

            var KTRContext = _context.Recipes.Include(s => s.StatusName).Include(c => c.CatName).Include(m => m.MainName);
            return View(await KTRContext.ToListAsync());
        }

        // ************************* GET: Recipes/Details/5 *********************
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

            ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName");
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "DisplayName");

            return View(recipes);
        }

        //  ********************* GET: Recipes/Create ****************************
        // [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName");
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "DisplayName");

            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,RecipeName,Description,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,PhotoPath,RegId")] Recipes recipes, IFormFile FilePhoto)
        {

            if (FilePhoto.FileName is null)
            {

                string photoPath = _webroot.WebRootPath + "\\FoodPhotos\\";
                var fileName = Path.GetFileName(FilePhoto.FileName);

                using (var stream = System.IO.File.Create(photoPath + fileName))
                {
                    await FilePhoto.CopyToAsync(stream);
                    recipes.PhotoPath = fileName;
                }
            }



            if (ModelState.IsValid)
            {
                _context.Add(recipes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "DisplayName", recipes.UserId);
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
            ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
            ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
            ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Email", "UserId", recipes.UserId);
            return View(recipes);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,RecipeName,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,Description,PhotoPath,RegId")] Recipes recipes, IFormFile FilePhoto)
        {
            if (id != recipes.RecipeId)
            {
                return NotFound();
            }

            if (FilePhoto.Length > 0)
            {

                string photoPath = _webroot.WebRootPath + "\\FoodPhotos\\";
                var fileName = Path.GetFileName(FilePhoto.FileName);

                using (var stream = System.IO.File.Create(photoPath + fileName))
                {
                    await FilePhoto.CopyToAsync(stream);
                    recipes.PhotoPath = fileName;
                }
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
            ViewData["UserId"] = new SelectList(_context.Users, "Email", "UserId", recipes.UserId);
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

        [Authorize]
        public IActionResult RecipeList()
        {
            return View();
        }


        // GET: Show Users their own recipes so they can edit them
        //[Authorize]
        //public async Task<IActionResult> ShowRecipes()
        //{

        //    string ShowId = _userManager.GetUserId(User);

        //    if (ShowId == null)
        //    {
        //        //return RedirectToAction("Recipes", "Index");
        //        return RedirectToAction("Recipes");
        //    }

        //    await _context.Recipes.ToListAsync();
        //    Recipes myRecipes = _context.Recipes.FirstOrDefault(p => p.RegId == ShowId);

        //    return View(myRecipes);

        //    //return View(await _context.Recipes.ToListAsync());

        //}

        // -----TRYING ANother Way - ERROR: ENUM IS ALWAYS NULL --------------

        [Authorize]
        public async Task<IActionResult> ShowRecipes()
        {

            string ShowId = _userManager.GetUserId(User);
            IEnumerable<Recipes> recipeEnum = new List<Recipes>();
            recipeEnum = _context.Recipes;

            if (ShowId == null)
            {
                //return RedirectToAction("Recipes", "Index");
                return RedirectToAction("Recipes");
            }
            else
            {
                foreach (Recipes r in recipeEnum)
                {
                    if (ShowId == r.RegId)
                    {
                        Recipes myRecipes = _context.Recipes.FirstOrDefault(p => p.RegId == ShowId);
                        await _context.Recipes.ToListAsync();
                        return View(myRecipes);
                    }
                    else
                    {
                        return NotFound();
                    }
                    
                }
                return NotFound();
            }
           
            //return View(myRecipes);
            

        }









        // Show Users ingredients for their recipes so they can edit them
        [Authorize]
        public async Task<IActionResult> ShowIngredients()
        {

            string ShowId = _userManager.GetUserId(User);
            Ingredients profile = _context.Ingredients.FirstOrDefault(p => p.IRegId == ShowId);

            if (profile == null)
            {
                return RedirectToAction("Recipes", "Index");
            }

            return View(await _context.Ingredients.ToListAsync());
            //return View(profile);

        }


        // Show Users their own directions so they can edit them
        [Authorize]
        public async Task<IActionResult> ShowPrep()
        {

            string ShowId = _userManager.GetUserId(User);
            Preparation profile = _context.Preparation.FirstOrDefault(p => p.PRegId == ShowId);

            if (profile == null)
            {
                return RedirectToAction("Recipes", "Index");
            }

            View(await _context.Preparation.ToListAsync());
            return View(profile);

        }





    }

}

