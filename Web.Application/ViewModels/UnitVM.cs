using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class UnitVM
    {
        public int Id { get; set; }
        [DisplayName("Название еденицы")]
        public string? Name { get; set; }

        public static List<UnitVM> GetUnits()
        {
            List<UnitVM> units = new List<UnitVM>();
           string  _query = "SELECT * FROM dbo.Unit_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        units.Add(
                            new UnitVM()
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name")
                            }
                        );
                    }
                }
            }

            return units;
        }

        public static Unit GetUnit(int? id)
        {
            Unit unit;
            string _query = "usp_Unit_SelectById";
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

                    unit = new Unit()
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name")
                    };
                }
            }

            return unit;
        }
    }
}
