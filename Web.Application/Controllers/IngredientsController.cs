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
    public class IngredientsController : Controller
    {
        private string? _query;
        private static int? _product;

        public IActionResult Index(int? product)
        {
            _product = product ?? ProductViewModel
                .GetProducts()
                .FirstOrDefault()!.Id;

            ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name");
            return View(IngredientViewModel.GetIngredients(_product));
        }

        /// get
        public IActionResult Create()
        {

            ViewData["Material"] = new SelectList(MaterialViewModel.GetMaterials(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _query = "usp_Ingredient_Insert";
                using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                    {
                        new SqlParameter("@product", _product),
                        new SqlParameter("@material", ingredient.Material),
                        new SqlParameter("@count", ingredient.Count)
                    };

                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                    sqlCommand.ExecuteNonQuery();
                }
                    

                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ingredient ingredient = IngredientViewModel.GetIngredient(id);

            ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name", ingredient.Product);
            ViewData["Material"] = new SelectList(MaterialViewModel.GetMaterials(), "Id", "Name", ingredient.Material);

            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _query = "usp_Ingredient_Update";
                    using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        List<SqlParameter> sqlParameters = new List<SqlParameter>()
                        {
                            new SqlParameter("@id", id),
                            new SqlParameter("@product", ingredient.Product),
                            new SqlParameter("@material", ingredient.Material),
                            new SqlParameter("@count", ingredient.Count)
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


            ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name", ingredient.Product);
            ViewData["Material"] = new SelectList(MaterialViewModel.GetMaterials(), "Id", "Name", ingredient.Material);

            return View(ingredient);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ingredient ingredient = IngredientViewModel.GetIngredient(id);
            Product product = ProductViewModel.GetProduct(ingredient.Product);
            Material material = MaterialViewModel.GetMaterial(ingredient.Material);


            ViewData["Product"] = product.Name;
            ViewData["Material"] = material.Name;
            return View(ingredient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _query = "usp_Ingredient_Delete";
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
