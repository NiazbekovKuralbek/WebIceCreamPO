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
    public class ProductionsController : Controller
    {
        private readonly DataBaseContext _context;

        public ProductionsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Production> productions = await _context.Productions
              .FromSqlRaw("Production_Select")
              .ToListAsync();

            return View(productions);
        }
        
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Production production)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@product", production.Product),
                    new SqlParameter("@count", production.Count),
                    new SqlParameter("@date", production.ProductionDate),
                    new SqlParameter("@employee", production.Employee)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Production_Insert @product, @count, @date, @employee",
                        sqlParameters.ToArray());

                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id", production.Employee);
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Id", production.Product);
            
            return View(production);
        }

    }
}
