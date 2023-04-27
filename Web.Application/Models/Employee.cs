using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "ФИО")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Должность")]
        public int? Position { get; set; }
        [Required]
        [Display(Name = "Зарплата")]
        public double? Salary { get; set; }
        [Display(Name = "Адрес")]
        public string? Address { get; set; }
        [Display(Name = "Номер Телофона")]
        public string? PhoneNumber { get; set; }
    }
}
