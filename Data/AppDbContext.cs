using Microsoft.EntityFrameworkCore;
using bayessoft.Models; 

namespace bayessoft.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<HizmetKategori> HizmetKategorileri { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Referans> Referanslar { get; set; }
        public DbSet<Icerik> Icerikler { get; set; }
        public DbSet<Ayar> Ayarlar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Admin>().ToTable("admin");
            modelBuilder.Entity<Ayar>().ToTable("ayarlar");
            modelBuilder.Entity<HizmetKategori>().ToTable("hizmet_kategorileri");
            modelBuilder.Entity<Hizmet>().ToTable("hizmetler");
            modelBuilder.Entity<Referans>().ToTable("referanslar");
            modelBuilder.Entity<Icerik>().ToTable("icerikler");

           
            modelBuilder.Entity<Hizmet>()
                .HasOne(h => h.Kategori)
                .WithMany(k => k.Hizmetler)
                .HasForeignKey(h => h.KategoriId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Username)
                .IsUnique();

            modelBuilder.Entity<Ayar>()
                .HasIndex(a => a.AyarAdi)
                .IsUnique();

            modelBuilder.Entity<HizmetKategori>()
                .HasIndex(k => k.BaglantiUzantisi)
                .IsUnique();

            modelBuilder.Entity<Hizmet>()
                .HasIndex(h => h.BaglantiUzantisi)
                .IsUnique();
        }
    }
}