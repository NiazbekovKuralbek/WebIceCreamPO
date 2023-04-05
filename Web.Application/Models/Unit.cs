using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Unit
    {
        public Unit()
        {
            Materials = new HashSet<Material>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
