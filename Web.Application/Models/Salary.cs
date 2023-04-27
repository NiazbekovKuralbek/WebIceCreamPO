using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Salary
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Сотрдуник")]
        public int? Employee { get; set; }
        [Required]
        [Display(Name = "Год")]
        public int? Year { get; set; }
        [Required]
        [Display(Name = "Месяц")]
        public int? Month { get; set; }
        [Display(Name = "В продажах")]
        public int? ForPurchase { get; set; }
        [Display(Name = "В производстве")]
        public int? ForProduction { get; set; }
        [Display(Name = "В продажах")]
        public int? ForSale { get; set; }
        [Display(Name = "Сырьё")]
        public int? General { get; set; }
        public double? BeforeSalary { get; set; }
        public double? Bonus { get; set; }
        public double? AfterSalary { get; set; }
        public string? Issued { get; set; }

    }
}
