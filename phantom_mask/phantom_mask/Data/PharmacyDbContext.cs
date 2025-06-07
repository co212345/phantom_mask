using Microsoft.EntityFrameworkCore;
using phantom_mask.Models;


namespace phantom_mask.Data
{
    public class PharmacyDbContext : DbContext
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base (options)
        {
        }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Mask> Masks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public DbSet<DailyOpeningHour> DailyOpeningHours { get; set; }
    }
}
