using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Month
    {
        public Month()
        {
            Salaries = new HashSet<Salary>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Salary> Salaries { get; set; }
    }
}
