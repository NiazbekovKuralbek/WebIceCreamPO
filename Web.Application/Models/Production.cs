namespace Web.Application.Models
{
    public class Production
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        public double? Count { get; set; }
        public DateTime? ProductionDate { get; set; }
        public int? Employee { get; set; }
    }
}
