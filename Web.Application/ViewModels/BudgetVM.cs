using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using Web.Application.Data;
using Web.Application.Models;

namespace Web.Application.ViewModels
{
    public class BudgetVM
    {
        public int Id { get; set; }
        [DisplayName("Бюджет")]
        public double? BudgetAmount { get; set; }
        [DisplayName("Процент")]
        public int? Percent { get; set; }
        [DisplayName("Бонус")]
        public int? Perks { get; set; }

        public static List<BudgetVM> GetBudgets()
        {
            List<BudgetVM> budgets = new List<BudgetVM>();
            string _query = "SELECT * FROM dbo.Budget_View";
            using (SqlCommand sqlCommand = new SqlCommand(_query, DataBaseContext.Connection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        budgets.Add(
                            new BudgetVM()
                            {
                                Id = reader.GetInt32("Id"),
                                BudgetAmount = reader.GetDouble("BudgetAmount"),
                                Percent = reader.GetInt32("Percent"),
                                Perks = reader.GetInt32("Perks")

                            }
                        );
                    }
                }
            }

            return budgets;
        }

        public static Budget GetBudget(int? id)
        {
            Budget budget;
            string _query = "usp_Budget_SelectById";
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

                    budget = new Budget()
                    {
                        Id = reader.GetInt32("Id"),
                        BudgetAmount = reader.GetDouble("budgetAmount"),
                        Percent = reader.GetInt32("Percent"),
                        Perks = reader.GetInt32("Perks")
                    };
                }
            }

            return budget;
        }

    }
}
