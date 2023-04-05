using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: SaleProducts
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.SaleProducts.Include(s => s.EmployeeNavigation).Include(s => s.ProductNavigation);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: SaleProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SaleProducts == null)
            {
                return NotFound();
            }

            var saleProduct = await _context.SaleProducts
                .Include(s => s.EmployeeNavigation)
                .Include(s => s.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleProduct == null)
            {
                return NotFound();
            }

            return View(saleProduct);
        }

        // GET: SaleProducts/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: SaleProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Product,Count,Amount,SaleDate,Employee")] SaleProduct saleProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saleProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id", saleProduct.Employee);
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Id", saleProduct.Product);
            return View(saleProduct);
        }

        // GET: SaleProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SaleProducts == null)
            {
                return NotFound();
            }

            var saleProduct = await _context.SaleProducts.FindAsync(id);
            if (saleProduct == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id", saleProduct.Employee);
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Id", saleProduct.Product);
            return View(saleProduct);
        }

        // POST: SaleProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product,Count,Amount,SaleDate,Employee")] SaleProduct saleProduct)
        {
            if (id != saleProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleProductExists(saleProduct.Id))
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
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id", saleProduct.Employee);
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Id", saleProduct.Product);
            return View(saleProduct);
        }

        // GET: SaleProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaleProducts == null)
            {
                return NotFound();
            }

            var saleProduct = await _context.SaleProducts
                .Include(s => s.EmployeeNavigation)
                .Include(s => s.ProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleProduct == null)
            {
                return NotFound();
            }

            return View(saleProduct);
        }

        // POST: SaleProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SaleProducts == null)
            {
                return Problem("Entity set 'DataBaseContext.SaleProducts'  is null.");
            }
            var saleProduct = await _context.SaleProducts.FindAsync(id);
            if (saleProduct != null)
            {
                _context.SaleProducts.Remove(saleProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleProductExists(int id)
        {
          return (_context.SaleProducts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
