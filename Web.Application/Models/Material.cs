using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Material
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название Сырья")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Еденица Измерения")]
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
