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
    public class PurchaseMaterialsController : Controller
    {
        private string? _query;

        public IActionResult Index()
        {
            return View(PurchaseMaterialVM.GetPurchaseMaterials());
        }
        
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(EmployeeVM.GetEmployees(), "Id", "Name");
            ViewData["Material"] = new SelectList(MaterialVM.GetMaterials(), "Id", "Name");

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PurchaseMaterial purchaseMaterial)
        {
            if (ModelState.IsValid)
            {

                _query = "usp_PurchaseMaterial_Insert";
                using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@return_value", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue},
                        new SqlParameter("@material", purchaseMaterial.Material),
                        new SqlParameter("@count", purchaseMaterial.Count),
                        new SqlParameter("@amount", purchaseMaterial.Amount),
                        new SqlParameter("@purchaseDate", purchaseMaterial.PurchaseDate),
                        new SqlParameter("@employee", purchaseMaterial.Employee)
                    };

                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlCommand.ExecuteNonQuery();

                    if ((int)sqlParameters[0].Value == 0)
                    {
                        return RedirectToAction(nameof(NotEnought));
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Employee"] = new SelectList(EmployeeVM.GetEmployees(), "Id", "Name", purchaseMaterial.Employee);
            ViewData["Material"] = new SelectList(MaterialVM.GetMaterials(), "Id", "Name", purchaseMaterial.Material);
            return View(purchaseMaterial);
        }

        public IActionResult NotEnought()
        {
            return View();
        }
    }
}
