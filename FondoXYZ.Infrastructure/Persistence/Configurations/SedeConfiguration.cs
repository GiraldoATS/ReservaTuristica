using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FondoXYZ.Infrastructure.Persistence.Configurations;

internal sealed class SedeConfiguration : IEntityTypeConfiguration<Sede>
{
    public void Configure(EntityTypeBuilder<Sede> builder)
    {
        builder.ToTable("Sedes");

        builder.HasKey(x => x.IdSede);

        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(Sede.LongitudMaximaNombre);

        builder.Property(x => x.Activa)
            .HasColumnName("Estado");
    }
}
