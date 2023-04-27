using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Web.Application.Data;
using Web.Application.Models;
using Web.Application.ViewModels;

namespace Web.Application.Controllers
{
    public class ProductsController : Controller
    {
        private string? _query;

        public IActionResult Index()
        {

            return View(ProductViewModel.GetProducts());
        }

        public IActionResult Create()
        {

            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product Product)
        {
            if (ModelState.IsValid)
            {
                _query = "usp_Product_Insert";
                using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@name", Product.Name),
                        new SqlParameter("@unit", Convert.ToInt32(Product.Unit)),
                        new SqlParameter("@count", Product.Count),
                        new SqlParameter("@amount", Product.Amount)
                    };

                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlCommand.ExecuteNonQuery();
                }


                return RedirectToAction(nameof(Index));
            }


            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name");
            return View(Product);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product Product = ProductViewModel.GetProduct(id);


            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name", Product.Unit);
            return View(Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product Product)
        {
            if (id != Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _query = "usp_Product_Update";
                    using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        List<SqlParameter> sqlParameters = new List<SqlParameter>()
                        {
                            new SqlParameter("@id", id),
                            new SqlParameter("@name", Product.Name),
                            new SqlParameter("@unit", Product.Unit),
                            new SqlParameter("@count", Product.Count),
                            new SqlParameter("@amount", Product.Amount)
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


            ViewData["Unit"] = new SelectList(UnitViewModel.GetUnits(), "Id", "Name", Product.Unit);
            return View(Product);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = ProductViewModel.GetProduct(id);
            Unit unit = UnitViewModel.GetUnit(product.Unit);


            ViewData["Unit"] = unit.Name;
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _query = "usp_Product_Delete";
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
