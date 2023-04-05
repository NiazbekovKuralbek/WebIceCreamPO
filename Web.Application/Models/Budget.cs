using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Budget
    {
        public int Id { get; set; }
        public double? BudgetAmount { get; set; }
        public int? Percent { get; set; }
        public int? Perks { get; set; }
    }
}
