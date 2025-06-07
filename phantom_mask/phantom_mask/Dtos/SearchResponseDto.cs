namespace phantom_mask.Dtos
{
    public class SearchResponseDto
    {
        public List<PharmacySearchResultDto> Pharmacies { get; set; }   = new List<PharmacySearchResultDto>();
        public List<MaskSearchResultDto> Masks { get; set; } = new List<MaskSearchResultDto>();
    }
}
