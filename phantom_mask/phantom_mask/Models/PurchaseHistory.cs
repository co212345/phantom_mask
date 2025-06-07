namespace phantom_mask.Models
{
    public class PurchaseHistory
    {
        public int PurchaseHistoryId { get; set; }
        public string PharmacyName { get; set; } = string.Empty;
        public string MaskName { get; set; } = string.Empty;
        public float TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int UserId { get; set; }
    }
}
