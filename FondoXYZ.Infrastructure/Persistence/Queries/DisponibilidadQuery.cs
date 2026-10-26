using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Queries;

/// <summary>
/// Adaptador del puerto de disponibilidad sobre el procedimiento sp_ConsultarDisponibilidad.
/// </summary>
internal sealed class DisponibilidadQuery : IDisponibilidadQuery
{
    private readonly ApplicationDbContext _context;

    public DisponibilidadQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<DisponibilidadDto>> ConsultarAsync(RangoFechas periodo, CancellationToken cancellationToken = default)
    {
        // SqlQuery con interpolación genera parámetros SQL (sin riesgo de inyección).
        var filas = await _context.Database
            .SqlQuery<DisponibilidadRow>(
                $"EXEC sp_ConsultarDisponibilidad @FechaInicio = {periodo.FechaInicio}, @FechaFin = {periodo.FechaFin}")
            .ToListAsync(cancellationToken);

        return filas
            .Select(f => new DisponibilidadDto(f.IdAlojamiento, f.Sede, f.Alojamiento, f.CapacidadMaxima, f.Tipo))
            .ToList();
    }

    private sealed class DisponibilidadRow
    {
        public int IdAlojamiento { get; set; }

        public string Sede { get; set; } = string.Empty;

        public string Alojamiento { get; set; } = string.Empty;

        public int CapacidadMaxima { get; set; }

        public string Tipo { get; set; } = string.Empty;
    }
}
