using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Material
    {
        public Material()
        {
            Ingredients = new HashSet<Ingredient>();
            PurchaseMaterials = new HashSet<PurchaseMaterial>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Unit { get; set; }
        public double? Count { get; set; }
        public double? Amount { get; set; }
        public double? Cost { get; set; }

        public virtual Unit UnitNavigation { get; set; } = null!;
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<PurchaseMaterial> PurchaseMaterials { get; set; }
    }
}
