using Microsoft.EntityFrameworkCore;
using DepartmanProjem.Models;

namespace DepartmanProjem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Birim> Birimler { get; set; }
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<Admin> Adminler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Birim ve Personel arasındaki ilişkiyi yapılandır
            modelBuilder.Entity<Personel>()
                .HasOne(p => p.Birim)
                .WithMany(b => b.Personeller)
                .HasForeignKey(p => p.BirimId)
                .OnDelete(DeleteBehavior.Cascade);

            // Admin tablosu için varsayılan veri ekle
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminId = 1,
                    KullaniciAd = "admin",
                    Sifre = "123456"
                }
            );

            // Örnek birim verileri ekle
            modelBuilder.Entity<Birim>().HasData(
                new Birim { BirimId = 1, BirimAd = "İnsan Kaynakları" },
                new Birim { BirimId = 2, BirimAd = "Bilgi İşlem" },
                new Birim { BirimId = 3, BirimAd = "Muhasebe" },
                new Birim { BirimId = 4, BirimAd = "Satış" },
                new Birim { BirimId = 5, BirimAd = "Müdür" }
            );
        }
    }
}
