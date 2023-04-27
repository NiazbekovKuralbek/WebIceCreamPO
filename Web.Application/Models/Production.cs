using System.ComponentModel.DataAnnotations;

namespace Web.Application.Models
{
    public class Production
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Продуктция")]
        public int? Product { get; set; }
        [Required]
        [Display(Name = "Кол-во")]
        public double? Count { get; set; }
        [Required]
        [Display(Name = "Дата прозводство")]
        public DateTime? ProductionDate { get; set; }
        [Required]
        [Display(Name = "Сотрудник")]
        public int? Employee { get; set; }
    }
}
