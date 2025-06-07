using Microsoft.VisualBasic;
using phantom_mask.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace phantom_mask.Data
{
    public class UserSeeder
    {
        public static async Task SeedAsync(PharmacyDbContext db, string jsonPath)
        {
            if (db.Users.Any()) return;

            var json = await File.ReadAllTextAsync(jsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonDateTimeConverter() }  
            };

            var rawList = JsonSerializer.Deserialize<List<RawUser>>(json, options)!;

            foreach(var raw in rawList)
            {
                var user = new User
                {
                    Name = raw.Name,
                    CashBalance = raw.CashBalance,
                    PurchaseHistories = raw.PurchaseHistories?.Select(p => new PurchaseHistory
                    {
                        MaskName = p.MaskName,
                        PharmacyName = p.PharmacyName,
                        TransactionAmount = p.TransactionAmount,
                        TransactionDate = p.TransactionDate,
                    }).ToList() ?? new List<PurchaseHistory>()
                };

                db.Users.Add(user);
            }
            await db.SaveChangesAsync();
        }
        private class RawUser
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
            [JsonPropertyName("cashBalance")]
            public float CashBalance { get; set; }
            [JsonPropertyName("purchaseHistories")]
            public List<RawPurchaseHistory> PurchaseHistories { get; set; } = new();
        }
        private class RawPurchaseHistory
        {
            [JsonPropertyName("pharmacyName")]
            public string PharmacyName { get; set; } = string.Empty;
            [JsonPropertyName("maskName")]
            public string MaskName {  get; set; } = string.Empty;
            [JsonPropertyName("transactionAmount")]
            public float TransactionAmount { get; set; }
            [JsonPropertyName("transactionDate")]
            public DateTime TransactionDate { get; set; }
        }
    }

}
