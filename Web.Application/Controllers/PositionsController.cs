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
    public class PositionsController : Controller
    {
        private readonly DataBaseContext _context;

        public PositionsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _context.Positions
              .FromSqlRaw("Position_Select")
              .ToListAsync();

            return View(positions);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@name", position.Name)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Position_Insert @name",
                        sqlParameters.ToArray());
                
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Positions == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };

            List<Position> positions = await _context.Positions
                .FromSqlRaw("EXEC Position_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Position position = positions.FirstOrDefault()!;
            
            return View(position);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Position position)
        {
            if (id != position.Id)
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
                        new SqlParameter("@name", position.Name)
                    };

                    await _context.Database
                        .ExecuteSqlRawAsync("EXEC Position_Update @id, @name",
                            sqlParameters.ToArray());
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(position.Id))
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
            return View(position);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Positions == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };

            List<Position> positions = await _context.Positions
                .FromSqlRaw("EXEC Position_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Position position = positions.FirstOrDefault()!;

            return View(position);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };

            await _context.Database
                .ExecuteSqlRawAsync("EXEC Position_Delete @id",
                    sqlParameters.ToArray());
            
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };

            List<Position> positions = _context.Positions
                .FromSqlRaw("EXEC Position_SelectById @id",
                    sqlParameters.ToArray())
                .ToList();

            return positions.FirstOrDefault() == null;
        }
    }
}
