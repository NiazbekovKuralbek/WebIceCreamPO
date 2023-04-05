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
    public class PurchaseMaterialsController : Controller
    {
        private readonly DataBaseContext _context;

        public PurchaseMaterialsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: PurchaseMaterials
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.PurchaseMaterials.Include(p => p.EmployeeNavigation).Include(p => p.MaterialNavigation);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: PurchaseMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseMaterials == null)
            {
                return NotFound();
            }

            var purchaseMaterial = await _context.PurchaseMaterials
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.MaterialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseMaterial == null)
            {
                return NotFound();
            }

            return View(purchaseMaterial);
        }

        // GET: PurchaseMaterials/Create
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Id");
            return View();
        }

        // POST: PurchaseMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Material,Count,Amount,PurchaseDate,Employee")] PurchaseMaterial purchaseMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id", purchaseMaterial.Employee);
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Id", purchaseMaterial.Material);
            return View(purchaseMaterial);
        }

        // GET: PurchaseMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseMaterials == null)
            {
                return NotFound();
            }

            var purchaseMaterial = await _context.PurchaseMaterials.FindAsync(id);
            if (purchaseMaterial == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id", purchaseMaterial.Employee);
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Id", purchaseMaterial.Material);
            return View(purchaseMaterial);
        }

        // POST: PurchaseMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Material,Count,Amount,PurchaseDate,Employee")] PurchaseMaterial purchaseMaterial)
        {
            if (id != purchaseMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseMaterialExists(purchaseMaterial.Id))
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
            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id", purchaseMaterial.Employee);
            ViewData["Material"] = new SelectList(_context.Materials, "Id", "Id", purchaseMaterial.Material);
            return View(purchaseMaterial);
        }

        // GET: PurchaseMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchaseMaterials == null)
            {
                return NotFound();
            }

            var purchaseMaterial = await _context.PurchaseMaterials
                .Include(p => p.EmployeeNavigation)
                .Include(p => p.MaterialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseMaterial == null)
            {
                return NotFound();
            }

            return View(purchaseMaterial);
        }

        // POST: PurchaseMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchaseMaterials == null)
            {
                return Problem("Entity set 'DataBaseContext.PurchaseMaterials'  is null.");
            }
            var purchaseMaterial = await _context.PurchaseMaterials.FindAsync(id);
            if (purchaseMaterial != null)
            {
                _context.PurchaseMaterials.Remove(purchaseMaterial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseMaterialExists(int id)
        {
          return (_context.PurchaseMaterials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
