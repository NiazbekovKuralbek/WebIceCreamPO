using Microsoft.Data.SqlClient;
using System.Data;
using Web.Application.Data;

namespace Web.Application.ViewModels
{
    public class PurchaseMaterialVM
    {
        public int Id { get; set; }
        public string? Material { get; set; }
        public double? Count { get; set; }
        public double? Amount { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Employee { get; set; }

        public static List<PurchaseMaterialVM> GetPurchaseMaterials()
        {
            List<PurchaseMaterialVM> purchaseMaterials = new List<PurchaseMaterialVM>();
            string _query = "SELECT * FROM dbo.PurchaseMaterial_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        purchaseMaterials.Add(
                            new PurchaseMaterialVM()
                            {
                                Id = reader.GetInt32("Id"),
                                Material = reader.GetString("Material"),
                                Count = reader.GetDouble("Count"),
                                Amount = reader.GetDouble("Amount"),
                                PurchaseDate = reader.GetDateTime("PurchaseDate"),
                                Employee = reader.GetString("Employee")

                            }
                        );
                    }
                }
            }

            return purchaseMaterials;
        }
    }
}
