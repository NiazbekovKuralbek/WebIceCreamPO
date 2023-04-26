using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class PurchaseMaterial
    {
        public int Id { get; set; }
        [Required]
        public int? Material { get; set; }
        [Required]
        public double? Count { get; set; }
        [Required]
        public double? Amount { get; set; }
        [Required]
        public DateTime? PurchaseDate { get; set; }
        [Required]
        public int? Employee { get; set; }

    }
}
