using System.ComponentModel.DataAnnotations;
using FondoXYZ.Application.DTOs;

namespace FondoXYZ.Web.Models.Reservas;

public class ReservaCreateViewModel
{
    public int IdAlojamiento { get; set; }

    public string NombreAlojamiento { get; set; } = string.Empty;

    public int CapacidadMaxima { get; set; }

    [Required(ErrorMessage = "La fecha inicio es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha fin es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime FechaFin { get; set; }

    [Range(1, 1000, ErrorMessage = "Debe ingresar mínimo 1 persona")]
    public int NumeroPersonas { get; set; } = 1;

    /// <summary>Solo informativo: el total definitivo siempre lo calcula el servidor.</summary>
    public decimal Total { get; set; }

    public CrearReservaCommand ToCommand(string idUsuario) =>
        new(idUsuario, IdAlojamiento, FechaInicio, FechaFin, NumeroPersonas);
}

public class DisponibilidadViewModel
{
    [DataType(DataType.Date)]
    public DateTime? FechaInicio { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaFin { get; set; }

    public bool Consultado { get; set; }

    public IReadOnlyList<DisponibilidadDto> Resultados { get; set; } = [];
}

public class CalcularTarifaRequest
{
    public int IdAlojamiento { get; set; }

    public int NumeroPersonas { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }
}
