using System;
using System.Collections.Generic;
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
using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using KTR.Models;
using KTR.ViewModels;
using System.IO;
using System.Web;



namespace KTR.Controllers
{
    public class RecipesController : Controller
    {
        private readonly KTRContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _webroot;
        private readonly IEnumerable<Users> _usersEnum;


        public RecipesController(KTRContext context, UserManager<IdentityUser> userManager, IHostingEnvironment webroot)
        {
            _context = context;
            _userManager = userManager;
            _webroot = webroot;
            _usersEnum = _context.Users;
            
            //_recipeList = recipeList;
        }


        // *********************************************************************************************************
        //                                           GET: Recipes  Index() 
        // *********************************************************************************************************

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

            //var UserRole = UserManager
            var CurrentUser = _userManager.GetUserName(User);

            //var UserRole= _userManager.GetUsersInRoleAsync("Administrator");
            
            //string ShowId = _userManager.GetUserId(User);
            
            if (CurrentUser == "admin@ktr.com")
                {
                return RedirectToAction("AdminIndex", "Recipes");
                }

                     


            var KTRContext = _context.Recipes.Include(s => s.StatusName).Include(c => c.CatName).Include(m => m.MainName);
            return View(await KTRContext.ToListAsync());
        }

        // *********************************************************************************************************
        //                          GET: Recipes  AdminIndex()  - FOR ADMINISTRATORS
        // *********************************************************************************************************

        [Authorize(Roles="Administrator")]

        public async Task<IActionResult> AdminIndex()
        {
          //ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
            ViewBag.CategoryId = (_context.RecipeCategory, "CategoryId", "CatName");
            //ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName");
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainName");
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
            ViewBag.UserId = (_context.Users, "UserId", "DisplayName");
            //ViewBag.UserId = new SelectList(_context.Users, "DisplayName");


            var KTRContext = _context.Recipes.Include(s => s.StatusName).Include(c => c.CatName).Include(m => m.MainName);
            return View(await KTRContext.ToListAsync());
        }






        // *********************************************************************************************************
        //                                        GET: Recipes/Details/5 
        // *********************************************************************************************************

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.SavedRecipeId = id;

            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
                .Include(r => r.CatName)
                .Include(r => r.MainName)
                .Include(r => r.StatusName)
                //.Include(r => r.description)
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

        // *********************************************************************************************************
        //                                       GET: Recipes/Create 
        // *********************************************************************************************************

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName");
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "DisplayName");

            return View();
        }

        // *********************************************************************************************************
        //                                          POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // *********************************************************************************************************

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeId,RecipeName,Description,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,RegId")] Recipes recipes, IFormFile FilePhoto)
        {

            if (FilePhoto.Length > 0 )
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
                //return RedirectToAction(nameof(Index));
                 return RedirectToAction(nameof(ShowRecipes));
                //return RedirectToAction("Create", "Ingredients");
            }
            ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "DisplayName", recipes.UserId);
            return View(recipes);
        }


        
        // *********************************************************************************************************
        //                                          GET: Recipes/Edit/5
        // *********************************************************************************************************

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.SavedRecipeId = id;

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
            //ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);

            return View(recipes);
            //return RedirectToAction(nameof(ShowRecipes));
        }


        // *********************************************************************************************************
        //                                                  POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // *********************************************************************************************************
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Orig_Edit(int id, [Bind("RecipeId,RecipeName,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,Description,RegId")] Recipes recipes,
            IFormFile FilePhoto)

        {
            ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
            ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
            ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "DisplayName", recipes.UserId);
            ViewBag.SavedRecipeId = id;

            recipes = await _context.Recipes.FindAsync(id);
            //recipes.RecipeId = id;
            if (id != recipes.RecipeId)
            {
                //return NotFound();
                return RedirectToAction(nameof(ShowRecipes));
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
                return RedirectToAction(nameof(ShowRecipes));
            }

            // return View(recipes);
               return RedirectToAction(nameof(ShowRecipes));
        }





        //     BROKEN  XXX_Edit
        // *********************************************************************************************************
        //                                                  POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // *********************************************************************************************************
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,RecipeName,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,Description,PhotoPath,RegId")] Recipes recipes, IFormFile FilePhoto)
        {
            ViewBag.SavedRecipeId = id;


            if (id != recipes.RecipeId)
                 {
                    return NotFound();
                 }
            // ***** Check and get filename
            if (FilePhoto == null)
            {
                recipes.PhotoPath = "DefaultFoodPhoto.png";
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
            ///recipes = await _context.Recipes.FindAsync(id);
            recipes.PhotoPath = "DefaultFoodPhoto.png";
            // ***** END  Check and get filename

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
                        return RedirectToAction(nameof(ShowRecipes));
                   }

                      ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName", recipes.CategoryId);
                      ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName", recipes.MainId);
                      ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName", recipes.StatusId);
                      ViewData["UserId"] = new SelectList(_context.Users, "UserId", "DisplayName", recipes.UserId);
                  
            return View(recipes);
            //return RedirectToAction(nameof(ShowRecipes));

         }

        // *********************************************************************************************************
        //                                          GET: Recipes/Admin Edit/5
        // *********************************************************************************************************      

        public async Task<IActionResult> AdminEdit(int? id)
        {
            ViewBag.SavedRecipeId = id;
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


        // *********************************************************************************************************
        //                                              POST: Recipes/AdminEdit/5
        // *********************************************************************************************************

        // POST: ExtraRecipes/AminEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEdit(int id, [Bind("RecipeId,RecipeName,UserId,Servings,CategoryId,StatusId,LastUpdated,MainId,Description,PhotoPath,RegId")] Recipes recipes)
        {
            ViewBag.SavedRecipeId = id;

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
            //return View(recipes);
            return RedirectToAction(nameof(ShowRecipes));
        }






        // *********************************************************************************************************
        //                                          GET: Recipes/Delete/5
        // *********************************************************************************************************

        [Authorize]
            public async Task<IActionResult> Delete(int? id)
            {
            ViewBag.SavedRecipeId = id;

            if (id == null)
               {
                return NotFound();
               }

            var recipes = await _context.Recipes
              .Include(r => r.CatName)
              .Include(r => r.MainName)
              .Include(r => r.StatusName)
              .Include(r => r.DisplayName)
               .FirstOrDefaultAsync(m => m.RecipeId == id);

            //var recipes = await _context.Recipes
            //    .FirstOrDefaultAsync(m => m.RecipeId == id);

            if (recipes == null)
               {
                return NotFound();
               }

                return View(recipes);
            }

        // *********************************************************************************************************
        //                                              POST: Recipes/Delete/5
        // *********************************************************************************************************

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipes = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowRecipes));
        }

        // *********************************************************************************************************
        private bool RecipesExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }




        // *********************************************************************************************************
        [Authorize]
        public IActionResult RecipeList()
        {
            return View();
        }


   // *************************************************************************************************
   //                                            SHOW RECIPES 
   // *************************************************************************************************

        [Authorize]
        public IActionResult ShowRecipes()
        {
            //ViewBag.CategoryId = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
            ViewBag.CategoryId = (_context.RecipeCategory, "CategoryId", "CatName");
            //ViewBag.MainId = new SelectList(_context.MainIngredient, "MainId", "MainName");
            ViewBag.MainId = new SelectList(_context.MainIngredient, "MainName");
            ViewBag.StatusId = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "DisplayName");
            //ViewBag.UserId = new SelectList(_context.Users, "DisplayName");

            string ShowId = _userManager.GetUserId(User);
           // IEnumerable<Recipes> recipeEnum = new List<Recipes>();
           // recipeEnum = _context.Recipes;
            KTRContext context = new KTRContext();
            // Recipes[] recipeLists =  _context.Recipes.ToArray();


            //Check for current user, send to admin page if admin is logged in. 
            var CurrentUser = _userManager.GetUserName(User);

             if (CurrentUser == "admin@ktr.com")
            {
                return RedirectToAction("AdminIndex", "Recipes");
            }



            if (ShowId != null)
           {
             
                var recipeList = (from r in context.Recipes
                              where r.RegId == ShowId
                             select r);
                //ViewBag.Data1 = recipeList.Include(s => s.StatusName).Include(c => c.CatName).Include(m => m.MainName);
                ViewBag.Data1 = recipeList.Include(s => s.StatusName).Include(c => c.CatName).Include(m => m.MainName);

                //ViewBag.EditCatName = (_context.RecipeCategory, "CategoryId", "CatName");
                //ViewBag.EditMainName = new SelectList(_context.MainIngredient, "MainName");
                //ViewBag.EditStatName = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
                //ViewBag.EditUserName = new SelectList(_context.Users, "UserId", "DisplayName");

                ViewData["CategoryId"] = new SelectList(_context.RecipeCategory, "CategoryId", "CatName");
                ViewData["MainId"] = new SelectList(_context.MainIngredient, "MainId", "MainName");
                ViewData["StatusId"] = new SelectList(_context.RecipeStatus, "StatusId", "StatusName");
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "DisplayName");

                return View();
             }

           return RedirectToAction("Home");
           


        }





        // *************************************************************************************************
        // Show Users ingredients for their recipes so they can edit them
        // *************************************************************************************************
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




