using Microsoft.Data.SqlClient;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class IngredientVM
    {
        public int Id { get; set; }
        public string? Product { get; set; }
        public string? Material { get; set; }
        public double? Count { get; set; }

        public static List<IngredientVM> GetIngredients(int? product)
        {
            List<IngredientVM> ingredients = new List<IngredientVM>();
            string _query = "usp_Ingredient_SelectByProduct";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@product", product)
                };

                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ingredients.Add(
                            new IngredientVM()
                            {
                                Id = reader.GetInt32("Id"),
                                Product = reader.GetString("Product"),
                                Material = reader.GetString("Material"),
                                Count = reader.GetDouble("Count"),

                            }
                        );
                    }
                }
            }

            return ingredients;
        } 

        public static Ingredient GetIngredient(int? id)
        {
            Ingredient ingredient;
            string _query = "usp_Ingredient_SelectById";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@id", id)
                };

                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    reader.Read();

                    ingredient = new Ingredient()
                    {
                        Id = reader.GetInt32("Id"),
                        Product = reader.GetInt32("Product"),
                        Material = reader.GetInt32("Material"),
                        Count = reader.GetDouble("Count")
                    };
                }
            }

            return ingredient;
        }

    }
}
