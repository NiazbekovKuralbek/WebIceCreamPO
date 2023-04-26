using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Production
    {
        public int Id { get; set; }
        [Required]
        public int? Product { get; set; }
        [Required]
        public double? Count { get; set; }
        [Required]
        public DateTime? ProductionDate { get; set; }
        [Required]
        public int? Employee { get; set; }
    }
}
