using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        [Required]
        public int? Material { get; set; }
        [Required]
        public double? Count { get; set; }

    }
}
