using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Unit
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название Ед.Измерения")]
        public string? Name { get; set; }
    }
}
