using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class PurchaseMaterial
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Сырьё")]
        public int? Material { get; set; }
        [Required]
        [Display(Name = "Кол-во")]
        public double? Count { get; set; }
        [Required]
        [Display(Name = "Цена")]
        public double? Amount { get; set; }
        [Required]
        [Display(Name = "Дата закупа")]
        public DateTime? PurchaseDate { get; set; }
        [Required]
        [Display(Name = "Сотрудник")]
        public int? Employee { get; set; }

    }
}
