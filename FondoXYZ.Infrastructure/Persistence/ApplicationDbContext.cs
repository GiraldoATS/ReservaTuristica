using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sede> Sedes => Set<Sede>();

    public DbSet<Alojamiento> Alojamientos => Set<Alojamiento>();

    public DbSet<Reserva> Reservas => Set<Reserva>();

    public DbSet<Tarifa> Tarifas => Set<Tarifa>();

    public DbSet<Temporada> Temporadas => Set<Temporada>();

    public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default) =>
        SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
