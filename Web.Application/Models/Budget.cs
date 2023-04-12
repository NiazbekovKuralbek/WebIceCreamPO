using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Web.Application.Data;

namespace Web.Application.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public double? BudgetAmount { get; set; }
        public int? Percent { get; set; }
        public int? Perks { get; set; }
    }
}
