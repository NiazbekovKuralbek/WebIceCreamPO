using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public class PurchaseMaterial
    {
        public int Id { get; set; }
        public int? Material { get; set; }
        public double? Count { get; set; }
        public double? Amount { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? Employee { get; set; }

    }
}
