using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FondoXYZ.Infrastructure.Persistence.Configurations;

internal sealed class TarifaConfiguration : IEntityTypeConfiguration<Tarifa>
{
    public void Configure(EntityTypeBuilder<Tarifa> builder)
    {
        builder.ToTable("Tarifas");

        builder.HasKey(x => x.IdTarifa);

        builder.Property(x => x.PrecioBase)
            .HasPrecision(18, 2);

        builder.Property(x => x.PrecioPersonaAdicional)
            .HasPrecision(18, 2);

        builder.Property(x => x.Activa)
            .HasColumnName("Estado");

        builder.HasOne(x => x.Alojamiento)
            .WithMany()
            .HasForeignKey(x => x.IdAlojamiento)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Temporada)
            .WithMany()
            .HasForeignKey(x => x.IdTemporada)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
