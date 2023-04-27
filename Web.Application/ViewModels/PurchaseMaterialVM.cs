using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Web.Application.Data;

namespace Web.Application.ViewModels
{
    public class PurchaseMaterialVM
    {
        public int Id { get; set; }

        [DisplayName("Материал")]
        public string? Material { get; set; }
        [DisplayName("Кол-во")]
        public double? Count { get; set; }
        [DisplayName("Цена")]
        public double? Amount { get; set; }
        [DisplayName("Дата закупа")]
        public DateTime? PurchaseDate { get; set; }

        [DisplayName("Сотрудник")]
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
