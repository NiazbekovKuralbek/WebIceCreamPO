using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly DataBaseContext _context;

        public MaterialsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Material> materials = await _context.Materials
                .FromSqlRaw("Material_Select")
                .ToListAsync();

            return View(materials);
        }
        
        public IActionResult Create()
        {
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@name", material.Name),
                    new SqlParameter("@unit", Convert.ToInt32(material.Unit)),
                    new SqlParameter("@count", material.Count),
                    new SqlParameter("@amount", material.Amount)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Material_Insert @name, @unit, @count, @amount",
                        sqlParameters.ToArray());

                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name", material.Unit);
            return View(material);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Material> materials = await _context.Materials
                .FromSqlRaw("EXEC Material_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Material material = materials.FirstOrDefault()!;

            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name", material.Unit);
            return View(material);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Material material)
        {
            if (id != material.Id)
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
                        new SqlParameter("@name", material.Name),
                        new SqlParameter("@unit", material.Unit),
                        new SqlParameter("@count", material.Count),
                        new SqlParameter("@amount", material.Amount)
                    };

                    await _context.Database
                        .ExecuteSqlRawAsync("EXEC Material_Update @id, @name, @unit, @count, @amount",
                            sqlParameters.ToArray());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
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
            
            ViewData["Unit"] = new SelectList(_context.Units, "Id", "Id", material.Unit);
            return View(material);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Material> materials = await _context.Materials
                .FromSqlRaw("EXEC Material_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Material material = materials.FirstOrDefault()!;

            return View(material);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Materials == null)
            {
                return Problem("Entity set 'DataBaseContext.Materials'  is null.");
            }
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            };

            List<Material> materials = _context.Materials
                .FromSqlRaw("EXEC Material_SelectById @id",
                    sqlParameters.ToArray())
                .ToList();
            
            return materials.FirstOrDefault() == null ;
        }
    }
}
