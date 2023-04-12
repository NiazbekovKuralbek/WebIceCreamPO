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
    public class SalariesController : Controller
    {
        private readonly DataBaseContext _context;

        public SalariesController(DataBaseContext context)
        {
            _context = context;
        }
        //не является рабочей версией
        
        public async Task<IActionResult> Index()
        {
            List<Salary> salaries = await _context.Salaries
              .FromSqlRaw("Salary_Select")
              .ToListAsync();

            return View(salaries);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };

            List<Salary> salaries = await _context.Salaries
                .FromSqlRaw("EXEC Salary_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Salary salary = salaries.FirstOrDefault()!;
                
            return View(salary);
        }
        
        public IActionResult Create()
        {
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Salary salary)
        {
            if (ModelState.IsValid)
            {
                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Salary_Insert");
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Name", salary.Month);
            
            return View(salary);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };

            List<Salary> salaries = await _context.Salaries
                .FromSqlRaw("EXEC Salary_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Salary salary = salaries.FirstOrDefault()!;
            
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Name", salary.Month);
            
            return View(salary);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Salary salary)
        {
            if (id != salary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        
                    };

                    await _context.Database
                        .ExecuteSqlRawAsync("EXEC Salary_Update",
                            sqlParameters.ToArray());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.Id))
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
            ViewData["Month"] = new SelectList(_context.Months, "Id", "Id", salary.Month);
            return View(salary);
        }

        private bool SalaryExists(int id)
        {
            return (_context.Salaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
