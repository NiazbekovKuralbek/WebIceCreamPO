using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Web.Application.Data;

namespace Web.Application.ViewModels
{
    public class SaleProductVM
    {
        public int Id { get; set; }
        [DisplayName("Продукция")]
        public string? Product { get; set; }
        [DisplayName("Кол-во")]
        public double? Count { get; set; }
        [DisplayName("Цена")]
        public double? Amount { get; set; }
        [DisplayName("Дата Продажи")]
        public DateTime? SaleDate { get; set; }
        [DisplayName("Сотрудник")]
        public string? Employee { get; set; }

        public static List<SaleProductVM> GetSaleProducts()
        {
            List<SaleProductVM> saleProducts = new List<SaleProductVM>();
            string _query = "SELECT * FROM dbo.SaleProduct_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        saleProducts.Add(
                            new SaleProductVM()
                            {
                                Id = reader.GetInt32("Id"),
                                Product = reader.GetString("Product"),
                                Count = reader.GetDouble("Count"),
                                Amount = reader.GetDouble("Amount"),
                                SaleDate = reader.GetDateTime("SaleDate"),
                                Employee = reader.GetString("Employee")

                            }
                        );
                    }
                }
            }

            return saleProducts;
        }
    }
}
