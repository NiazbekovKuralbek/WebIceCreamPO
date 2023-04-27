using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название продукции")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Еденица Сырья")]
        public int? Unit { get; set; }
        [Required]
        [Display(Name = "Кол-во")]
        public double? Count { get; set; }
        [Required]
        [Display(Name = "Цена")]
        public double? Amount { get; set; }
        [Display(Name = "Себестоимость")]
        public double? Cost { get; set; }
    }
}