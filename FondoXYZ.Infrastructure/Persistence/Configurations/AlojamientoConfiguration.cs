using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FondoXYZ.Infrastructure.Persistence.Configurations;

internal sealed class AlojamientoConfiguration : IEntityTypeConfiguration<Alojamiento>
{
    public void Configure(EntityTypeBuilder<Alojamiento> builder)
    {
        builder.ToTable("Alojamientos");

        builder.HasKey(x => x.IdAlojamiento);

        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(Alojamiento.LongitudMaximaNombre);

        builder.Property(x => x.Activo)
            .HasColumnName("Estado");

        builder.HasOne(x => x.Sede)
            .WithMany()
            .HasForeignKey(x => x.IdSede)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
