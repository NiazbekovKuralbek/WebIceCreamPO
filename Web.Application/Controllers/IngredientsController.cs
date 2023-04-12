using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly DataBaseContext _context;
        private static int _product;

        public IngredientsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index(int? product)
        {
            List<Product> products = await _context.Products
                .FromSqlRaw("EXEC Product_Select")
                .ToListAsync();

            _product = product ?? products.FirstOrDefault()!.Id;
            
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@product", _product)
            };

            List<Ingredient> ingredients = await _context.Ingredients
                .FromSqlRaw("EXEC Ingredient_SelectByProduct @product",
                    sqlParameters.ToArray())
                .ToListAsync();

            ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            return View(ingredients);
        }
        
        public IActionResult Create()
        {
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@product", ingredient.Product),
                    new SqlParameter("@material", ingredient.Material),
                    new SqlParameter("@count", ingredient.Count)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Ingredient_Insert @product, @material, @count",
                        sqlParameters.ToArray());
                
                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Ingredient> ingredients = await _context.Ingredients
                .FromSqlRaw("EXEC Ingredient_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Ingredient ingredient = ingredients.FirstOrDefault()!;

            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name", ingredient.Material);
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Name", ingredient.Product);
            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@id", id),
                        new SqlParameter("@product", ingredient.Product),
                        new SqlParameter("@material", ingredient.Material),
                        new SqlParameter("@count", ingredient.Count)
                    };

                    await _context.Database
                        .ExecuteSqlRawAsync("EXEC Ingredient_Update @id, @product, @material, @count",
                            sqlParameters.ToArray());

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.Id))
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
            
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name");
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            
            return View(ingredient);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Ingredient> ingredients = await _context.Ingredients
                .FromSqlRaw("EXEC Ingredient_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Ingredient ingredient = ingredients.FirstOrDefault()!;
            
            return View(ingredient);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            await _context.Database
                .ExecuteSqlRawAsync("EXEC Ingredient_Delete @id",
                    sqlParameters.ToArray());

            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Ingredient> ingredients = _context.Ingredients
                .FromSqlRaw("EXEC Ingredient_SelectById @id",
                    sqlParameters.ToArray())
                .ToList();
            
            return ingredients.FirstOrDefault() == null;
        }
    }
}
