using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Productions = new HashSet<Production>();
            PurchaseMaterials = new HashSet<PurchaseMaterial>();
            SaleProducts = new HashSet<SaleProduct>();
            Salaries = new HashSet<Salary>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Position { get; set; }
        public double? Salary { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual Position PositionNavigation { get; set; } = null!;
        public virtual ICollection<Production> Productions { get; set; }
        public virtual ICollection<PurchaseMaterial> PurchaseMaterials { get; set; }
        public virtual ICollection<SaleProduct> SaleProducts { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
    }
}
