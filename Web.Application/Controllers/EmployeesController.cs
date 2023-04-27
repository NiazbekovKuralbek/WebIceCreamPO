using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;
using Web.Application.ViewModels;

namespace Web.Application.Controllers
{
    public class EmployeesController : Controller
    {
        private string? _query;
        public IActionResult Index()
        {

            return View(EmployeeViewModel.GetEmployees());
        }
        
        public IActionResult Create()
        {

            ViewData["Position"] = new SelectList(PositionViewModel.GetPositions(), "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _query = "usp_Employee_Insert";
                using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@name", employee.Name),
                        new SqlParameter("@position", employee.Position),
                        new SqlParameter("@salary", employee.Salary),
                        new SqlParameter("@address", employee.Address),
                        new SqlParameter("@phone", employee.PhoneNumber)
                    };

                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlCommand.ExecuteNonQuery();
                }


                return RedirectToAction(nameof(Index));
            }


            ViewData["Position"] = new SelectList(PositionViewModel.GetPositions(), "Id", "Name");
            return View();
        }
        
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = EmployeeViewModel.GetEmployee(id);


            ViewData["Position"] = new SelectList(PositionViewModel.GetPositions(), "Id", "Name", employee.Position);
            return View(employee);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            try
            {
                _query = "usp_Employee_Update";
                using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@id", id),
                        new SqlParameter("@name", employee.Name),
                        new SqlParameter("@position", employee.Position),
                        new SqlParameter("@salary", employee.Salary),
                        new SqlParameter("@address", employee.Address),
                        new SqlParameter("@phone", employee.PhoneNumber)
                    };

                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlCommand.ExecuteNonQuery();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }


            ViewData["Position"] = new SelectList(PositionViewModel.GetPositions(), "Id", "Name", employee.Position);
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = EmployeeViewModel.GetEmployee(id);
            Position position = PositionViewModel.GetPosition(employee.Position);


            ViewData["Position"] = position.Name;
            return View(employee);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Employee? employee)
        {
            if (employee != null)
            {
                _query = "usp_Employee_Delete";
                using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@id", employee.Id)
                    };

                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlCommand.ExecuteNonQuery();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
