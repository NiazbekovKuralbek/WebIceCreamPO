using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DataBaseContext _context;

        public EmployeesController(DataBaseContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees
                .FromSqlRaw("Employee_Select")
                .ToListAsync();
            
            List<Position> positions = await _context.Positions
                .FromSqlRaw("Position_Select")
                .ToListAsync();
            
            
            return View(employees);
        }
        
        public async Task<IActionResult> Create()
        {
            List<Position> positions = await _context.Positions
                .FromSqlRaw("Position_Select")
                .ToListAsync();
            
            ViewData["Position"] = new SelectList(positions, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@name", employee.Name),
                    new SqlParameter("@position", employee.Position),
                    new SqlParameter("@salary", employee.Salary),
                    new SqlParameter("@address", employee.Address),
                    new SqlParameter("@phone", employee.PhoneNumber)
                };
                
                await _context.Database
                    .ExecuteSqlRawAsync(@"EXEC Employee_Insert @name, @position, @salary, @address, @phone",
                        sqlParameters.ToArray());

                return RedirectToAction(nameof(Index));
            }
            
            List<Position> positions = await _context.Positions
                .FromSqlRaw("Position_Select")
                .ToListAsync();
            
            ViewData["Position"] = new SelectList(positions, "Id", "Name", employee.Position);
            return View(employee);
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

            List<Employee> employees = await _context.Employees
                .FromSqlRaw("EXEC Employee_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Employee employee = employees.FirstOrDefault()!;

            List<Position> positions = await _context.Positions
                .FromSqlRaw("Position_Select")
                .ToListAsync();
            
            ViewData["Position"] = new SelectList(positions, "Id", "Name", employee.Position);
            return View(employee);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@id", id),
                        new SqlParameter("@name", employee.Name),
                        new SqlParameter("@position", employee.Position),
                        new SqlParameter("@salary", employee.Salary),
                        new SqlParameter("@address", employee.Address),
                        new SqlParameter("@phone", employee.PhoneNumber)
                    };

                    await _context.Database
                        .ExecuteSqlRawAsync("EXEC Employee_Update @id, @name, @position, @salary, @address, @phone",
                            sqlParameters.ToArray());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewData["Position"] = new SelectList(_context.Positions, "Id", "Name", employee.Position);
                return RedirectToAction(nameof(Index));
            }
            
            
            return View(employee);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", id)
            };

            List<Employee> employees = await _context.Employees
                .FromSqlRaw("EXEC Employee_SelectById @id",
                    sqlParameters.ToArray())
                .ToListAsync();

            Employee employee = employees.FirstOrDefault()!;

            return View(employee);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Employee? employee)
        {
            if (employee != null)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", employee.Id)
                };

                await _context.Database
                    .ExecuteSqlRawAsync("EXEC Employee_Delete @id",
                        sqlParameters.ToArray());
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("id", id)
            };

            List<Employee> employee = _context.Employees
                .FromSqlRaw("EXEC Employee_SelectById @id",
                    sqlParameters.ToArray())
                .ToList();

            return employee.FirstOrDefault() != null;
        }
    }
}
