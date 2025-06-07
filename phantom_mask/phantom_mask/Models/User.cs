using System.Collections.ObjectModel;

namespace phantom_mask.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public float CashBalance { get; set; }
        public ICollection<PurchaseHistory> PurchaseHistories { get; set; } = new HashSet<PurchaseHistory>();
    }
}
