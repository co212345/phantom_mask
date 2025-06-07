namespace phantom_mask.Models
{
    public class Mask
    {
        public int MaskId { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public int PharmacyId { get; set; }               
    }
}
