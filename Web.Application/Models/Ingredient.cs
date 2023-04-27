using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Display(Name = "Продукт")]
        public int? Product { get; set; }
        [Required]
        [Display(Name = "Сырьё")]
        public int? Material { get; set; }
        [Required]
        [Display(Name = "Кол-во")]
        public double? Count { get; set; }

    }
}
