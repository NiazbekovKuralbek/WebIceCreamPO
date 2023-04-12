using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Position { get; set; }
        public double? Salary { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
