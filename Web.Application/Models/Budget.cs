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
        [Display(Name = "Бюджет")]
        public double? BudgetAmount { get; set; }
        [Required]
        [Display(Name = "Процент")]
        public int? Percent { get; set; }
        [Required]
        [Display(Name = "Бонус")]
        public int? Perks { get; set; }
    }
}
