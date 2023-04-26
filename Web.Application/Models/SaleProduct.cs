using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class SaleProduct
    {
        public int Id { get; set; }
        [Required]
        public int? Product { get; set; }
        [Required]
        public double? Count { get; set; }
        [Required]
        public double? Amount { get; set; }
        [Required]
        public DateTime? SaleDate { get; set; }
        [Required]
        public int? Employee { get; set; }
    }
}
