// Gerekli kütüphaneleri ekliyoruz
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAT242516081ITHELPDESK.Data; // ApplicationUser sýnýfýný bulmak için
using SAT242516081ITHELPDESK.Models; // Kategori, Talep vb. modelleri bulmak için

// Namespace'i hocanýzýn þablonuna [image_8fa39d.png] göre düzeltiyoruz
namespace SAT242516081ITHELPDESK.Models.DbContexts
{
    // Sýnýf adýný hocanýzýn þablonuna [image_8fa39d.png, 84] göre düzeltiyoruz
    public class MyDbModel_DbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor (Kurucu) adýný da yeni sýnýf adýyla güncelliyoruz
        public MyDbModel_DbContext(DbContextOptions<MyDbModel_DbContext> options)
            : base(options)
        {
        }

        // Helpdesk projemizin tüm tablolarý (DbSet'leri)
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Durum> Durumlar { get; set; }
        public DbSet<Talep> Talepler { get; set; }
        public DbSet<TalepGuncelleme> TalepGuncellemeleri { get; set; }

        // Zincirleme silme (cascade delete) hatasýný
        // çözmek için eklediðimiz kod
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Talep>()
                .HasOne(t => t.OlusturanKullanici)
                .WithMany()
                .HasForeignKey(t => t.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Talep>()
                .HasOne(t => t.AtananTeknisyen)
                .WithMany()
                .HasForeignKey(t => t.AtananTeknisyenId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TalepGuncelleme>()
                .HasOne(g => g.GuncelleyenKullanici)
                .WithMany()
                .HasForeignKey(g => g.GuncelleyenKullaniciId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}