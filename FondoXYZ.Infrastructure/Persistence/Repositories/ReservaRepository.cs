using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Enums;
using FondoXYZ.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Repositories;

internal sealed class ReservaRepository : IReservaRepository
{
    private readonly ApplicationDbContext _context;

    public ReservaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Reserva>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Reservas
            .AsNoTracking()
            .Include(x => x.Alojamiento)
            .OrderByDescending(x => x.IdReserva)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExisteCruceAsync(int idAlojamiento, RangoFechas periodo, CancellationToken cancellationToken = default)
    {
        // Misma regla que RangoFechas.SeCruzaCon, expresada para que se traduzca a SQL.
        var inicio = periodo.FechaInicio;
        var fin = periodo.FechaFin;

        return _context.Reservas.AnyAsync(
            r => r.IdAlojamiento == idAlojamiento
                && r.Estado == EstadoReserva.Activa
                && inicio < r.FechaFin
                && r.FechaInicio < fin,
            cancellationToken);
    }

    public Task<bool> ExisteParaAlojamientoAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        _context.Reservas.AnyAsync(x => x.IdAlojamiento == idAlojamiento, cancellationToken);

    public void Agregar(Reserva reserva) => _context.Reservas.Add(reserva);
}
