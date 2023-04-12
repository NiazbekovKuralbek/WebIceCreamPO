using System;
using System.Collections;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        public int? Material { get; set; }
        public double? Count { get; set; }
    }
}
