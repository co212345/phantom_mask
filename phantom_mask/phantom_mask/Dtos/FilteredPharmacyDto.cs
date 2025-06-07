namespace phantom_mask.Dtos
{
    public class FilteredPharmacyDto
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaskCount { get; set; }
    }
}
