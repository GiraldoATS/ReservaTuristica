using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Queries;

/// <summary>
/// Adaptador del puerto de cálculo de tarifa sobre el procedimiento sp_CalcularTarifa.
/// </summary>
internal sealed class CalculoTarifaQuery : ICalculoTarifaQuery
{
    private readonly ApplicationDbContext _context;

    public CalculoTarifaQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CalculoTarifaDto?> CalcularAsync(
        int idAlojamiento,
        int numeroPersonas,
        RangoFechas periodo,
        CancellationToken cancellationToken = default)
    {
        var filas = await _context.Database
            .SqlQuery<CalculoTarifaRow>(
                $"""
                EXEC sp_CalcularTarifa
                    @IdAlojamiento = {idAlojamiento},
                    @FechaInicio = {periodo.FechaInicio},
                    @FechaFin = {periodo.FechaFin},
                    @NumeroPersonas = {numeroPersonas}
                """)
            .ToListAsync(cancellationToken);

        var fila = filas.FirstOrDefault();

        // El SP siempre devuelve una fila; si no encontró temporada o tarifa, sus columnas vienen en NULL.
        if (fila?.IdTemporada is null || fila.PrecioBase is null || fila.TotalPagar is null)
            return null;

        return new CalculoTarifaDto(
            fila.IdTemporada.Value,
            fila.PrecioBase.Value,
            fila.PrecioPersonaAdicional ?? 0,
            fila.NumeroNoches ?? periodo.Noches,
            fila.PersonasAdicionales ?? 0,
            fila.TotalPagar.Value);
    }

    private sealed class CalculoTarifaRow
    {
        public int? IdTemporada { get; set; }

        public decimal? PrecioBase { get; set; }

        public decimal? PrecioPersonaAdicional { get; set; }

        public int? NumeroNoches { get; set; }

        public int? PersonasAdicionales { get; set; }

        public decimal? TotalPagar { get; set; }
    }
}
