using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FondoXYZ.Infrastructure.Persistence.Configurations;

internal sealed class TemporadaConfiguration : IEntityTypeConfiguration<Temporada>
{
    public void Configure(EntityTypeBuilder<Temporada> builder)
    {
        builder.ToTable("Temporadas");

        builder.HasKey(x => x.IdTemporada);

        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(Temporada.LongitudMaximaNombre);

        builder.Property(x => x.Activa)
            .HasColumnName("Estado");
    }
}
