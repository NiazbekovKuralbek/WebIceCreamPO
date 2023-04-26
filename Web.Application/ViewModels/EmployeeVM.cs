using Microsoft.Data.SqlClient;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public double? Salary { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }


        public static List<EmployeeVM> GetEmployees()
        {
            List<EmployeeVM> employees = new List<EmployeeVM>();
            string _query = "SELECT * FROM dbo.[Employee_View]";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(
                            new EmployeeVM()
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Position = reader.GetString("Position"),
                                Salary = reader.GetDouble("Salary"),
                                Address = reader.GetString("Address"),
                                PhoneNumber = reader.GetString("PhoneNumber")
                            }
                        );
                    }
                }
            }

            return employees;
        }
    
        public static Employee GetEmployee(int? id)
        {
            Employee employee;
            string _query = "usp_Employee_SelectById";
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

                    employee = new Employee()
                    {
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        Position = reader.GetInt32("Position"),
                        Salary = reader.GetDouble("Salary"),
                        Address = reader.GetString("Address"),
                        PhoneNumber = reader.GetString("PhoneNumber")
                    };

                }
            }

            return employee;
        }
    }
}
