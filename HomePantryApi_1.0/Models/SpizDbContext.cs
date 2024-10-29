using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HomePantryApi_1._0.Models
{
    public partial class SpizDbContext : DbContext
    {
        public SpizDbContext() { }

        public SpizDbContext(DbContextOptions<SpizDbContext> options)
            : base(options) { }

        public virtual DbSet<Granary> Granaries { get; set; }
        public virtual DbSet<Productsingranary> Productsingranaries { get; set; }
        public virtual DbSet<Productsinshoplist> Productsinshoplists { get; set; }
        public virtual DbSet<Shoplist> Shoplists { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql("server=localhost;database=spizdb;user=root;password=Admin1;port=3306", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Granary>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("granaries");

                entity.HasIndex(e => e.UserId, "UserId");

                entity.Property(e => e.DataAktualizacji)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("datetime")
                    .HasColumnName("dataAktualizacji");
                entity.Property(e => e.DataUtworzenia)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("datetime")
                    .HasColumnName("dataUtworzenia");
                entity.Property(e => e.GranaryName).HasMaxLength(20);
                entity.Property(e => e.Opis)
                    .HasColumnType("text")
                    .HasColumnName("opis");

                entity.HasOne(d => d.User).WithMany(p => p.Granaries)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("granaries_ibfk_1");
            });

            modelBuilder.Entity<Productsingranary>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PRIMARY");
                entity.ToTable("productsingranary");

                entity.HasIndex(e => e.GranaryId, "GranaryId");

                entity.Property(e => e.Cena)
                    .HasPrecision(10, 2)
                    .HasDefaultValueSql("'0.00'")
                    .HasColumnName("cena");
                entity.Property(e => e.DataWaznosci).HasColumnName("dataWaznosci");
                entity.Property(e => e.DataZakupu).HasColumnName("dataZakupu");
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.ProductName).HasMaxLength(255);
                entity.Property(e => e.Rodzaj)
                    .HasMaxLength(255)
                    .HasColumnName("rodzaj");
                entity.Property(e => e.Weight).HasPrecision(10, 2);

                entity.HasOne(d => d.Granary).WithMany(p => p.Productsingranaries)
                    .HasForeignKey(d => d.GranaryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("productsingranary_ibfk_1");
            });

            modelBuilder.Entity<Shoplist>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("shoplists");

                entity.HasIndex(e => e.UserId, "UserId");

                entity.Property(e => e.DataAktualizacji)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("datetime")
                    .HasColumnName("dataAktualizacji");
                entity.Property(e => e.DataUtworzenia)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("datetime")
                    .HasColumnName("dataUtworzenia");
                entity.Property(e => e.Opis)
                    .HasColumnType("text")
                    .HasColumnName("opis");
                entity.Property(e => e.ShoplistName).HasMaxLength(20);

                entity.HasOne(d => d.User).WithMany(p => p.Shoplists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("shoplists_ibfk_1");
            });

            modelBuilder.Entity<Productsinshoplist>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PRIMARY");
                entity.ToTable("productsinshoplist");

                entity.HasIndex(e => e.ShoplistId, "ShoplistId");

                entity.Property(e => e.Cena)
                    .HasPrecision(10, 2)
                    .HasDefaultValueSql("'0.00'")
                    .HasColumnName("cena");
                entity.Property(e => e.DataWaznosci).HasColumnName("dataWaznosci");
                entity.Property(e => e.DataZakupu).HasColumnName("dataZakupu");
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.ProductName).HasMaxLength(255);
                entity.Property(e => e.Rodzaj)
                    .HasMaxLength(255)
                    .HasColumnName("rodzaj");
                entity.Property(e => e.Weight).HasPrecision(10, 2);

                entity.HasOne(d => d.Shoplist).WithMany(p => p.Productsinshoplists)
                    .HasForeignKey(d => d.ShoplistId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("productsinshoplist_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("users");

                entity.Property(e => e.Email)
                    .HasMaxLength(255) // Zwiększona długość
                    .IsRequired(); // Upewnij się, że to wymagane
                entity.HasIndex(e => e.Email).IsUnique(); // Dodaj ograniczenie unikalności

                entity.Property(e => e.Login)
                    .HasMaxLength(255) // Zwiększona długość
                    .IsRequired(); // Upewnij się, że to wymagane
                entity.HasIndex(e => e.Login).IsUnique(); // Dodaj ograniczenie unikalności

                entity.Property(e => e.Password)
                    .HasMaxLength(60) // Zwiększona długość
                    .IsRequired(); // Upewnij się, że to wymagane
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
