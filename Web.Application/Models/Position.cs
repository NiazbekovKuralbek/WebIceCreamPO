﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
