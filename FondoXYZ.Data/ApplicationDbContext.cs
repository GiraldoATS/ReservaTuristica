using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sede> Sedes { get; set; }

        public DbSet<Alojamiento> Alojamientos { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Tarifa> Tarifas { get; set; }

        public DbSet<Temporada> Temporadas { get; set; }
        public DbSet<DisponibilidadDto> Disponibilidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DisponibilidadDto>()
                .HasNoKey();

            // =========================
            // SEDES
            // =========================

            modelBuilder.Entity<Sede>()
                .HasKey(x => x.IdSede);

            // =========================
            // ALOJAMIENTOS
            // =========================

            modelBuilder.Entity<Alojamiento>()
                .HasKey(x => x.IdAlojamiento);

            modelBuilder.Entity<Alojamiento>()
                .HasOne(x => x.Sede)
                .WithMany(x => x.Alojamientos)
                .HasForeignKey(x => x.IdSede);

            // =========================
            // RESERVAS
            // =========================

            modelBuilder.Entity<Reserva>()
                .HasKey(x => x.IdReserva);

            modelBuilder.Entity<Reserva>()
                .Property(x => x.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reserva>()
                .HasOne(x => x.Alojamiento)
                .WithMany(x => x.Reservas)
                .HasForeignKey(x => x.IdAlojamiento);

            // =========================
            // TARIFAS
            // =========================

            modelBuilder.Entity<Tarifa>()
                .HasKey(x => x.IdTarifa);

            modelBuilder.Entity<Tarifa>()
                .Property(x => x.PrecioBase)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Tarifa>()
                .Property(x => x.PrecioPersonaAdicional)
                .HasPrecision(18, 2);

            // =========================
            // TEMPORADAS
            // =========================

            modelBuilder.Entity<Temporada>()
                .HasKey(x => x.IdTemporada);
        }
    }
}