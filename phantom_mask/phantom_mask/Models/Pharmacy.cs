namespace phantom_mask.Models
{
    public class Pharmacy
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public float CashBalance { get; set; }
        public ICollection<DailyOpeningHour> OpeningHours { get; set; } = new HashSet<DailyOpeningHour>();
        public ICollection<Mask> Masks { get; set; } = new HashSet<Mask>();
    }
}
