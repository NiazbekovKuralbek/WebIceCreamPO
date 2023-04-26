using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Salary
    {
        public int Id { get; set; }
        [Required]
        public int? Employee { get; set; }
        [Required]
        public int? Year { get; set; }
        [Required]
        public int? Month { get; set; }
        public int? ForPurchase { get; set; }
        public int? ForProduction { get; set; }
        public int? ForSale { get; set; }
        public int? General { get; set; }
        public double? BeforeSalary { get; set; }
        public double? Bonus { get; set; }
        public double? AfterSalary { get; set; }
        public string? Issued { get; set; }

    }
}
