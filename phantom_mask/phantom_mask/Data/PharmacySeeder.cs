using phantom_mask.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace phantom_mask.Data
{
    public static class PharmacySeeder
    {
        public static async Task SeedAsync(PharmacyDbContext db, string jsonPath)
        {
            if (db.Pharmacies.Any()) return;

            var json = await File.ReadAllTextAsync(jsonPath);
            var rawList = JsonSerializer.Deserialize<List<RawPharmacy>>(json)!;

            foreach (var raw in rawList)
            {
                var pharmacy = new Pharmacy
                {
                    Name = raw.Name,
                    CashBalance = raw.CashBalance,
                    Masks = raw.Masks.Select(m => new Mask
                    {
                        Name = m.Name,
                        Price = m.Price,
                    }).ToList()
                };

                
                db.Pharmacies.Add(pharmacy);
                await db.SaveChangesAsync();

                var hours = OpeningHourParser.Parse(raw.OpeningHours);
                foreach (var hour in hours)
                {
                    hour.PharmacyId = pharmacy.PharmacyId;
                }
                db.DailyOpeningHours.AddRange(hours);
                await db.SaveChangesAsync();
            }
        }

        private class RawPharmacy
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
            [JsonPropertyName("cashBalance")]
            public float CashBalance { get; set; }
            [JsonPropertyName("openingHours")]
            public string OpeningHours { get; set; } = string.Empty;
            [JsonPropertyName("masks")]
            public List<RawMask> Masks { get; set; } = new();
        }

        private class RawMask
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
            [JsonPropertyName("price")]
            public float Price { get; set; }
        }
    }
}
