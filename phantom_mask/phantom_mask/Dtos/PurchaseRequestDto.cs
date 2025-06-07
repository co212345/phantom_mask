namespace phantom_mask.Dtos
{
    public class PurchaseItemDto
    {
        public string PharmacyName { get; set; } = string.Empty;
        public string MaskName { get; set; } = string.Empty ;
        public float TransactionAmount { get; set; }
    }

    public class PurchaseRequestDto
    {
        public int UserId { get; set; }
        public List<PurchaseItemDto> Items { get; set; } = new();
    }

}
