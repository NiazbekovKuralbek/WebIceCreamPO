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
    public class SaleProductsController : Controller
    {
        private readonly DataBaseContext _context;

        public SaleProductsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<SaleProduct> sales = await _context.SaleProducts
              .FromSqlRaw("SaleProduct_Select")
              .ToListAsync();

            return View(sales);
        }

        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleProduct saleProduct)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@product", saleProduct.Product),
                    new SqlParameter("@count", saleProduct.Count),
                    new SqlParameter("@amount", saleProduct.Amount),
                    new SqlParameter("@date", saleProduct.SaleDate),
                    new SqlParameter("@employee", saleProduct.Employee)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC SaleProduct_Insert @product, @count, @amount, @date, @employee",
                        sqlParameters.ToArray());
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name", saleProduct.Employee);
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Name", saleProduct.Product);
            return View(saleProduct);
        }
    }
}
