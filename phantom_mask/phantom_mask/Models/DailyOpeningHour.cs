namespace phantom_mask.Models
{
    public class DailyOpeningHour
    {
        public int Id { get; set; } 
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int PharmacyId { get; set; }
    }
}
