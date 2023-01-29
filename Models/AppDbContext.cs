using System;
using System.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;

namespace TvRaspored.Models
{
    public partial class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Emisija> Emisija { get; set; }
        public virtual DbSet<Korisnik> Korisnik { get; set; }
        public virtual DbSet<Pretplata> Pretplata { get; set; }
        public virtual DbSet<TipKorisnika> TipKorisnika { get; set; }
        public virtual DbSet<Tvpostaja> Tvpostaja { get; set; }
        public virtual DbSet<Zanr> Zanr { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["TVRASPOREDDBConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Emisija>(entity =>
            {
                entity.HasIndex(e => e.TvpostajaId)
                    .HasName("fk_emisija_tvpostaja1_idx");

                entity.HasIndex(e => e.ZanrId)
                    .HasName("fk_emisija_zanr1_idx");

                entity.Property(e => e.Naziv).IsUnicode(false);

                entity.Property(e => e.Opis).IsUnicode(false);

                entity.HasOne(d => d.Tvpostaja)
                    .WithMany(p => p.Emisija)
                    .HasForeignKey(d => d.TvpostajaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_emisija_tvpostaja1");

                entity.HasOne(d => d.Zanr)
                    .WithMany(p => p.Emisija)
                    .HasForeignKey(d => d.ZanrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_emisija_zanr1");
            });

            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Ime).IsUnicode(false);

                entity.Property(e => e.KorisnickoIme).IsUnicode(false);

                entity.Property(e => e.Prezime).IsUnicode(false);

                entity.Property(e => e.Slika).IsUnicode(false);

                entity.HasOne(d => d.Tip)
                    .WithMany(p => p.Korisnik)
                    .HasForeignKey(d => d.TipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_korisnik_tip_korisnika");
            });

            modelBuilder.Entity<Pretplata>(entity =>
            {
                entity.HasIndex(e => e.EmisijaId)
                    .HasName("fk_korisnik_has_emisija_emisija1_idx");

                entity.HasIndex(e => e.KorisnikId)
                    .HasName("fk_korisnik_has_emisija_korisnik1_idx");

                entity.HasOne(d => d.Emisija)
                    .WithMany(p => p.Pretplata)
                    .HasForeignKey(d => d.EmisijaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_korisnik_has_emisija_emisija1");

                entity.HasOne(d => d.Korisnik)
                    .WithMany(p => p.Pretplata)
                    .HasForeignKey(d => d.KorisnikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_korisnik_has_emisija_korisnik1");
            });

            modelBuilder.Entity<TipKorisnika>(entity =>
            {
                entity.HasKey(e => e.TipId)
                    .HasName("pk_tip_korisnika");

                entity.Property(e => e.Naziv).IsUnicode(false);
            });

            modelBuilder.Entity<Tvpostaja>(entity =>
            {
                entity.HasIndex(e => e.ModeratorId)
                    .HasName("fk_tvpostaja_korisnik1_idx");

                entity.Property(e => e.Naziv).IsUnicode(false);

                entity.HasOne(d => d.Moderator)
                    .WithMany(p => p.Tvpostaja)
                    .HasForeignKey(d => d.ModeratorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tvpostaja_korisnik1");
            });

            modelBuilder.Entity<Zanr>(entity =>
            {
                entity.Property(e => e.Naziv).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
