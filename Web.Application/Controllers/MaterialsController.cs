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
    public class MaterialsController : Controller
    {
        private string? _query; 

        public IActionResult Index()
        {
            
            return View(MaterialViewModel.GetMaterials());
        }

        public IActionResult Create()
        {

            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Material material)
        {
            if (ModelState.IsValid)
            {
                _query = "usp_Material_Insert";
                using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@name", material.Name),
                        new SqlParameter("@unit", material.Unit),
                        new SqlParameter("@count", material.Count),
                        new SqlParameter("@amount", material.Amount)
                    };

                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlCommand.ExecuteNonQuery();
                }


                return RedirectToAction(nameof(Index));
            }

            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name");
            return View(material);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Material material = MaterialViewModel.GetMaterial(id);


            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name", material.Unit);
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _query = "usp_Material_Update";
                    using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        List<SqlParameter> sqlParameters = new List<SqlParameter>()
                        {
                            new SqlParameter("@id", id),
                            new SqlParameter("@name", material.Name),
                            new SqlParameter("@unit", material.Unit),
                            new SqlParameter("@count", material.Count),
                            new SqlParameter("@amount", material.Amount)
                        };

                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                        sqlCommand.ExecuteNonQuery();
                    }
                        

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name", material.Unit);
            return View(material);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Material material = MaterialViewModel.GetMaterial(id);
            Unit unit = UnitViewModel.GetUnit(material.Unit);


            ViewData["Unit"] = unit.Name;
            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _query = "usp_Material_Delete";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@id", id)
                };

                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                sqlCommand.ExecuteNonQuery();
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
