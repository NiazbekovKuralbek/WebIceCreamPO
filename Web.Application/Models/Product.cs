using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Product
    {
        public Product()
        {
            Ingredients = new HashSet<Ingredient>();
            Productions = new HashSet<Production>();
            SaleProducts = new HashSet<SaleProduct>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Unit { get; set; }
        public double? Count { get; set; }
        public double? Amount { get; set; }
        public double? Cost { get; set; }

        public virtual Unit UnitNavigation { get; set; } = null!;
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Production> Productions { get; set; }
        public virtual ICollection<SaleProduct> SaleProducts { get; set; }
    }
}
