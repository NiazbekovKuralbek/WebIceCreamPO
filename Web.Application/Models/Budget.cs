using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Application.Data;

namespace Web.Application.Models
{
    public class Budget
    {
        public int Id { get; set; }
        [Required]
        public double? BudgetAmount { get; set; }
        [Required]
        public int? Percent { get; set; }
        [Required]
        public int? Perks { get; set; }
    }
}
