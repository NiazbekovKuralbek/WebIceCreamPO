using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.Application.Models
{
    public class Month
    {
        public int Id { get; set; }
        [Display(Name = "Месяц")]
        public string? Name { get; set; }
    }
}
