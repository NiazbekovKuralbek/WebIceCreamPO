using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Unit { get; set; }
        [Required]
        public double? Count { get; set; }
        [Required]
        public double? Amount { get; set; }
        public double? Cost { get; set; }
    }
}