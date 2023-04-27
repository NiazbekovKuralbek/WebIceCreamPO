using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class MaterialVM
    {
        public int Id { get; set; }
        [DisplayName("Сырьё")]
        public string? Name { get; set; }
        [DisplayName("Ед.Измерения")]
        public string? Unit { get; set; }
        [DisplayName("Кол-во")]
        public double? Count { get; set; }
        [DisplayName("Цена")]
        public double? Amount { get; set; }
        [DisplayName("Себестоимость")]
        public double? Cost { get; set; }

        public static List<MaterialVM> GetMaterials()
        {
            List<MaterialVM> materials = new List<MaterialVM>();
            string _query = "SELECT * FROM dbo.Material_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        materials.Add(
                            new MaterialVM()
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

                return materials;
            }
        }

        public static Material GetMaterial(int? id)
        {
            Material material;
            string _query = "usp_Material_SelectById";
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

                    material = new Material()
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Unit = reader.GetInt32("Unit"),
                        Count = reader.GetDouble("Count"),
                        Amount = reader.GetDouble("Amount")
                     
                    };
                }
            }

            return material;
        }
    }
}
