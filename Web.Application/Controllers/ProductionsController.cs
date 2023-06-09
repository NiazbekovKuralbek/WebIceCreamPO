﻿using System;
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
    public class ProductionsController : Controller
    {
        private string? _query;

        public IActionResult Index()
        {
            
            return View(ProductionViewModel.GetProductions());
        }
        
        public IActionResult Create()
        {
            ViewData["Employee"] = new SelectList(EmployeeViewModel.GetEmployees(), "Id", "Name");
            ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name");

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Production production)
        {
            int retrurn_value = 0;
             
            if (ModelState.IsValid)
            {
                try
                {
                    _query = "usp_Production_Insert";
                    using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("return_value", retrurn_value) { Direction = ParameterDirection.ReturnValue},
                        new SqlParameter("@product", production.Product),
                        new SqlParameter("@productCount", production.Count),
                        new SqlParameter("@productionDate", production.ProductionDate),
                        new SqlParameter("@employee", production.Employee)
                    };
                        /*добавить условие проверки return_value*/

                        sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                        sqlCommand.ExecuteNonQuery();
                    }


                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex) 
                {
                    ViewData["Alredy"] = "Не хватает сырья!";
                }
            }


            ViewData["Employee"] = new SelectList(EmployeeViewModel.GetEmployees(), "Id", "Name", production.Employee);
            ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name", production.Product);
            return View(production);
        }

    }
}
