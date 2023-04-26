using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Position { get; set; }
        [Required]
        public double? Salary { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
