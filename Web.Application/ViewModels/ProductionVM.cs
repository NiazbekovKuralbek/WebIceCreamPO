using Microsoft.Data.SqlClient;
using System.Data;
using Web.Application.Data;

namespace Web.Application.ViewModels
{
    public class ProductionVM
    {
        public int Id { get; set; }
        public string? Product { get; set; }
        public double? Count { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string? Employee { get; set; }

        public static List<ProductionVM> GetProductions()
        {
            List<ProductionVM> productions = new List<ProductionVM>();
            string _query = "SELECT * FROM dbo.Production_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productions.Add(
                            new ProductionVM()
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
