using Microsoft.Data.SqlClient;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class PositionVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public static List<PositionVM> GetPositions()
        {
            List<PositionVM> positions = new List<PositionVM>();
            string _query = "SELECT * FROM dbo.[Position_View]";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        positions.Add(
                            new PositionVM()
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name")
                            }
                        );
                    }
                }

            }

            return positions;
        }

        public static Position GetPosition(int? id)
        {
            Position position;
            string _query = "usp_Position_SelectById";
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

                    position = new Position()
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name")
                    };
                }
            }

            return position;
        }

    }
}
