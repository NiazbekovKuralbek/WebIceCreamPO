using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class SaleProduct
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Продукция")]
        public int? Product { get; set; }
        [Required]
        [Display(Name = "Кол-во")]
        public double? Count { get; set; }
        [Required]
        [Display(Name = "Цена")]
        public double? Amount { get; set; }
        [Required]
        [Display(Name = "Дата продажи")]
        public DateTime? SaleDate { get; set; }
        [Required]
        [Display(Name = "Сотрудник")]
        public int? Employee { get; set; }
    }
}
