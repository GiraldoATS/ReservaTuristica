using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Enums;
using FondoXYZ.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FondoXYZ.Infrastructure.Persistence.Configurations;

internal sealed class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.ToTable("Reservas");

        builder.HasKey(x => x.IdReserva);

        builder.Ignore(x => x.Periodo);

        builder.Property(x => x.IdUsuario)
            .IsRequired();

        builder.Property(x => x.Total)
            .HasPrecision(18, 2);

        // Se conserva el formato existente en la base de datos ("ACTIVA"), que también usan los SPs.
        builder.Property(x => x.Estado)
            .IsRequired()
            .HasConversion(
                estado => estado.ToString().ToUpper(),
                valor => Enum.Parse<EstadoReserva>(valor, true));

        builder.HasOne(x => x.Alojamiento)
            .WithMany()
            .HasForeignKey(x => x.IdAlojamiento)
            .OnDelete(DeleteBehavior.Restrict);

        // El dominio solo conoce el Id del usuario; la relación con Identity vive en infraestructura.
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
