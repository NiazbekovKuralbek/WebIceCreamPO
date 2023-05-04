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
        // Здесь объявляем переменные, которые будут использоваться внутри класса
        private string? _query;
        private static int? _product;

        // Экшен, который вызывается при запросе /Ingredients/Index
        // Если указан параметр product, то будут отображены только ингредиенты для соответствующего продукта
        // Если параметр не указан, то будет выбран первый продукт из списка и отображены все ингредиенты для него
        public IActionResult Index(int? product)
        {
            _product = product ?? ProductViewModel
                .GetProducts()
                .FirstOrDefault()!.Id;

            // Получаем список продуктов и передаем их в выпадающий список в представлении
            ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name");

            // Получаем список ингредиентов для выбранного продукта и передаем их в представление
            return View(IngredientViewModel.GetIngredients(_product));
        }

        // Экшен, который вызывается при запросе /Ingredients/Create (GET)
        // Он открывает страницу с формой для создания нового ингредиента
        public IActionResult Create()
        {
            // Сохраняем выбранный продукт для передачи в представление
            ViewData["_product"] = _product;

            // Получаем список материалов и передаем их в выпадающий список в представлении
            ViewData["Material"] = new SelectList(MaterialViewModel.GetMaterials(), "Id", "Name");

            return View();
        }

        // Экшен, который вызывается при отправке формы на странице создания нового ингредиента (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ingredient ingredient)
        {
            // Сохраняем выбранный продукт для передачи в представление
            ViewData["_product"] = _product;

            if (ModelState.IsValid)
            {
                try
                {
                    // Если записи еще нет, то добавляем ее в базу данных
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
                }
                catch (Exception ex)
                {
                    // Если запись уже существует, то передаем сообщение об ошибке в представление
                    ViewData["Alredy"] = "Данная сырьё уже присутствует в ингридиенте";
                    ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name", ingredient.Product);
                    ViewData["Material"] = new SelectList(MaterialViewModel.GetMaterials(), "Id", "Name", ingredient.Material); return View(ingredient);
                }
                return RedirectToAction(nameof(Index), new { product = _product});
            }

            return View(ingredient);
        }

        public IActionResult Edit(int? id)
        {
            ViewData["_product"] = _product;
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
            ViewData["_product"] = _product;
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
                return RedirectToAction(nameof(Index), new { product = _product });
            }


            ViewData["Product"] = new SelectList(ProductViewModel.GetProducts(), "Id", "Name", ingredient.Product);
            ViewData["Material"] = new SelectList(MaterialViewModel.GetMaterials(), "Id", "Name", ingredient.Material);

            return View(ingredient);
        }

        public IActionResult Delete(int? id)
        {
            ViewData["_product"] = _product;
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
            ViewData["_product"] = _product;
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

            return RedirectToAction(nameof(Index), new { product = _product });
        }
    }
}
