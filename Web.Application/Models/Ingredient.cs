using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Ingredient
    {
        public int Id { get; set; }
        public int Product { get; set; }
        public int Material { get; set; }
        public double? Count { get; set; }

        public virtual Material MaterialNavigation { get; set; } = null!;
        public virtual Product ProductNavigation { get; set; } = null!;
    }
}
