using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region DbSets

        // Entidades
        public DbSet<Sede> Sedes { get; set; }

        public DbSet<Alojamiento> Alojamientos { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Tarifa> Tarifas { get; set; }

        public DbSet<Temporada> Temporadas { get; set; }

        // DTOs
        public DbSet<DisponibilidadDto> Disponibilidades { get; set; }

        public DbSet<CalculoTarifaDto> CalculoTarifas { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureDtos(modelBuilder);
            ConfigureSede(modelBuilder);
            ConfigureAlojamiento(modelBuilder);
            ConfigureReserva(modelBuilder);
            ConfigureTarifa(modelBuilder);
            ConfigureTemporada(modelBuilder);
            ConfigureIdentity(modelBuilder);
        }

        #region Configurations

        private static void ConfigureDtos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DisponibilidadDto>()
                .HasNoKey();

            modelBuilder.Entity<CalculoTarifaDto>()
                .HasNoKey();

            modelBuilder.Entity<CalculoTarifaDto>()
                .Property(x => x.PrecioBase)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CalculoTarifaDto>()
                .Property(x => x.PrecioPersonaAdicional)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CalculoTarifaDto>()
                .Property(x => x.TotalPagar)
                .HasPrecision(18, 2);
        }

        private static void ConfigureSede(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sede>()
                .HasKey(x => x.IdSede);

            modelBuilder.Entity<Sede>()
                .Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(150);
        }

        private static void ConfigureAlojamiento(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alojamiento>()
                .HasKey(x => x.IdAlojamiento);

            modelBuilder.Entity<Alojamiento>()
                .Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Alojamiento>()
                .HasOne(x => x.Sede)
                .WithMany(x => x.Alojamientos)
                .HasForeignKey(x => x.IdSede)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureReserva(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reserva>()
                .HasKey(x => x.IdReserva);

            modelBuilder.Entity<Reserva>()
                .Property(x => x.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reserva>()
                .HasOne(x => x.Alojamiento)
                .WithMany(x => x.Reservas)
                .HasForeignKey(x => x.IdAlojamiento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureTarifa(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarifa>()
                .HasKey(x => x.IdTarifa);

            modelBuilder.Entity<Tarifa>()
                .Property(x => x.PrecioBase)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Tarifa>()
                .Property(x => x.PrecioPersonaAdicional)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Tarifa>()
                .HasOne(x => x.Alojamiento)
                .WithMany()
                .HasForeignKey(x => x.IdAlojamiento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarifa>()
                .HasOne(x => x.Temporada)
                .WithMany()
                .HasForeignKey(x => x.IdTemporada)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureTemporada(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Temporada>()
                .HasKey(x => x.IdTemporada);

            modelBuilder.Entity<Temporada>()
                .Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(100);
        }

        private static void ConfigureIdentity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .Property(x => x.NombreCompleto)
                .HasMaxLength(150);
        }

        #endregion
    }
}