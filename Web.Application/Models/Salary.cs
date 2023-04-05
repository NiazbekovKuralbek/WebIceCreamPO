using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Salary
    {
        public int Id { get; set; }
        public int? Employee { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? ForPurchase { get; set; }
        public int? ForProduction { get; set; }
        public int? ForSale { get; set; }
        public int? General { get; set; }
        public double? BeforeSalary { get; set; }
        public double? Bonus { get; set; }
        public double? AfterSalary { get; set; }
        public string? Issued { get; set; }

        public virtual Month? MonthNavigation { get; set; }
        public virtual Employee? EmployeeNavigation { get; set; }
    }
}
