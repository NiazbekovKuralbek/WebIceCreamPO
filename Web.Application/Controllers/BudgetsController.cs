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
    public class BudgetsController : Controller
    {
        private readonly DataBaseContext _context;

        public BudgetsController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Budget> budget = await _context.Budgets
                .FromSqlRaw("Budget_Select")
                .ToListAsync();

            return View(budget);
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

            List<Budget> budget = await _context.Budgets
                .FromSqlRaw("EXEC Budget_SelectById @id", sqlParameters.ToArray())
                .ToListAsync();

            return View(budget.First());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@budget", budget.BudgetAmount),
                        new SqlParameter("@percent", budget.Percent),
                        new SqlParameter("@perks", budget.Perks)
                    };
                    
                    await Task.Run(() => _context.Database
                        .ExecuteSqlRawAsync(@"EXEC Budget_Update @budget, @percent, @perks", sqlParameters.ToArray()));

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }
    }
}
