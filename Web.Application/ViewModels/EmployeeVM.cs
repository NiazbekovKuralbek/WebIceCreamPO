using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }
        [DisplayName("ФИО")]
        public string? Name { get; set; }
        [DisplayName("Должность")]
        public string? Position { get; set; }
        [DisplayName("Зарплата")]
        public double? Salary { get; set; }
        [DisplayName("Адрес")]
        public string? Address { get; set; }
        [DisplayName("НомерТелефона")]
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
