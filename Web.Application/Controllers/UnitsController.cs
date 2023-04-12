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
    public class UnitsController : Controller
    {
        private readonly DataBaseContext _context;

        public UnitsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Unit> units = await _context.Units
                .FromSqlRaw("unit_Select")
                .ToListAsync();

            return View(units);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Unit unit)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@name", unit.Name)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Unit_Insert @name",
                        sqlParameters.ToArray());
                
                return RedirectToAction(nameof(Index));
            }
            return View(unit);
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

            List<Unit> units = await _context.Units
                .FromSqlRaw("EXEC Unit_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Unit unit = units.FirstOrDefault()!;
            
            return View(unit);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Unit unit)
        {
            if (id != unit.Id)
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
                        new SqlParameter("@name", unit.Name)
                    };

                    await _context.Database
                        .ExecuteSqlRawAsync("EXEC Unit_Update @id, @name",
                            sqlParameters.ToArray());
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.Id))
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
            return View(unit);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Unit> units = await _context.Units
                .FromSqlRaw("EXEC Unit_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Unit unit = units.FirstOrDefault()!;

            return View(unit);
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
                .ExecuteSqlRawAsync("EXEC Unit_Delete @id",
                    sqlParameters.ToArray());

            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
            return (_context.Units?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
