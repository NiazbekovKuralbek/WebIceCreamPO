using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string? Name { get; set; }
        [DisplayName("Ед.Измерение")]
        public string? Unit { get; set; }
        [DisplayName("Кол-во")]
        public double? Count { get; set; }
        [DisplayName("Цена")]
        public double? Amount { get; set; }
        [DisplayName("Себестоимость")]
        public double? Cost { get; set; }
        
        public static List<ProductViewModel> GetProducts()
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            string _query = "SELECT * FROM dbo.Product_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(
                            new ProductViewModel()
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Unit = reader.GetString("Unit"),
                                Count = reader.GetDouble("Count"),
                                Amount = reader.GetDouble("Amount"),
                                Cost = reader.GetDouble("Cost")

                            }
                        );
                    }
                }
            }

            return products;
        }

        public static Product GetProduct(int? id)
        {
            Product product;
            string _query = "usp_Product_SelectById";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;

                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("id", id)
                };

                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    reader.Read();

                    product = new Product()
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Unit = reader.GetInt32("Unit"),
                        Count = reader.GetDouble("Count"),
                        Amount = reader.GetDouble("Amount")
                    };
                }
            }

            return product;
        }

    }
}
