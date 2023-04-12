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
    public class PurchaseMaterialsController : Controller
    {
        private readonly DataBaseContext _context;

        public PurchaseMaterialsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<PurchaseMaterial> purchases = await _context.PurchaseMaterials
              .FromSqlRaw("PurchaseMaterial_Select")
              .ToListAsync();

            return View(purchases);
        }
        
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name");
            
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseMaterial purchaseMaterial)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@material", purchaseMaterial.Material),
                    new SqlParameter("@count", purchaseMaterial.Count),
                    new SqlParameter("@amount", purchaseMaterial.Amount),
                    new SqlParameter("@date", purchaseMaterial.PurchaseDate),
                    new SqlParameter("@employee", purchaseMaterial.Employee)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC PurchaseMaterial_Insert @material, @count, @amount, @date, @employee",
                        sqlParameters.ToArray());

                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", purchaseMaterial.Employee);
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name", purchaseMaterial.Material);
            return View(purchaseMaterial);
        }
    }
}
