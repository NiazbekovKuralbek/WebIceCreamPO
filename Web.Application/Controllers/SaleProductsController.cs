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
    public class SaleProductsController : Controller
    {
        private string? _query;

        public IActionResult Index()
        {
            return View(SaleProductVM.GetSaleProducts());
        }

        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(EmployeeVM.GetEmployees(), "Id", "Name");
            ViewData["Product"] = new SelectList(ProductVM.GetProducts(), "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SaleProduct saleProduct)
        {
            _query = "usp_SaleProduct_Insert";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@product", saleProduct.Product),
                    new SqlParameter("@count", saleProduct.Count),
                    new SqlParameter("@saleDate", saleProduct.SaleDate),
                    new SqlParameter("@employee", saleProduct.Employee)
                };


                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                sqlCommand.ExecuteNonQuery();
            }

            return RedirectToAction(nameof(Index));
            
        }
    }
}
