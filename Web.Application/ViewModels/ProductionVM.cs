using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Web.Application.Data;

namespace Web.Application.ViewModels
{
    public class ProductionViewModel
    {
        public int Id { get; set; }
        [DisplayName("Продукция")]
        public string? Product { get; set; }
        [DisplayName("Кол-во")]
        public double? Count { get; set; }
        [DisplayName("Дата Произ-ва")]
        public DateTime? ProductionDate { get; set; }
        [DisplayName("Сотрудник")]
        public string? Employee { get; set; }

        public static List<ProductionViewModel> GetProductions()
        {
            List<ProductionViewModel> productions = new List<ProductionViewModel>();
            string _query = "SELECT * FROM dbo.Production_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productions.Add(
                            new ProductionViewModel()
                            {
                                Id = reader.GetInt32("Id"),
                                Product = reader.GetString("Product"),
                                Count = reader.GetDouble("Count"),
                                ProductionDate = reader.GetDateTime("ProductionDate"),
                                Employee = reader.GetString("Employee")

                            }
                        );
                    }
                }
            }

            return productions;
        }
    }
}
