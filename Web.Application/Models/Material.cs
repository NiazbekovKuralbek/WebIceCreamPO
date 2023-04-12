using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Unit { get; set; }
        public double? Count { get; set; }
        public double? Amount { get; set; }
        public double? Cost { get; set; }
    }
}
