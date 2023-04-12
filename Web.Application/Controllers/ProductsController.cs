using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataBaseContext _context;

        public ProductsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
              .FromSqlRaw("Product_Select")
              .ToListAsync();

            return View(products);
        }

        
        public IActionResult Create()
        {
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@name", product.Name),
                    new SqlParameter("@unit", product.Unit),
                    new SqlParameter("@count", product.Count),
                    new SqlParameter("@amount", product.Amount),
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Product_Insert @name, @unit, @count, @amount",
                        sqlParameters.ToArray());
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name", product.Unit);
            return View(product);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Product> products = await _context.Products
                .FromSqlRaw("EXEC Product_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Product product = products.FirstOrDefault()!;
            
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Id", product.Unit);
            return View(product);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
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
                        new SqlParameter("@name", product.Name),
                        new SqlParameter("@unit", product.Unit),
                        new SqlParameter("@count", product.Count),
                        new SqlParameter("@amount", product.Amount),
                    };

                    await _context.Database
                        .ExecuteSqlRawAsync("EXEC Product_Update @id, @name, @unit, @count, @amount",
                            sqlParameters.ToArray());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Id", product.Unit);
            return View(product);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Product> products = await _context.Products
                .FromSqlRaw("EXEC Product_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Product product = products.FirstOrDefault()!;

            return View(product);
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
                .ExecuteSqlRawAsync("EXEC Product_Delete @id",
                    sqlParameters.ToArray());

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Product> products = _context.Products
                .FromSqlRaw("EXEC Product_SelectById @id",
                    sqlParameters.ToArray())
                .ToList();

            return products.FirstOrDefault() == null;
        }
    }
}
