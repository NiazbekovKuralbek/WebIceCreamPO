using System;
using System.Collections.Generic;

namespace Web.Application.Models
{
    public partial class Production
    {
        public int Id { get; set; }
        public int Product { get; set; }
        public double? Count { get; set; }
        public DateTime? ProductionDate { get; set; }
        public int Employee { get; set; }

        public virtual Employee EmployeeNavigation { get; set; } = null!;
        public virtual Product ProductNavigation { get; set; } = null!;
    }
}
