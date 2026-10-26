using System.ComponentModel.DataAnnotations;
using FondoXYZ.Application.DTOs;

namespace FondoXYZ.Web.Models;

public class TarifaFormViewModel
{
    public int IdTarifa { get; set; }

    [Required(ErrorMessage = "Seleccione un alojamiento")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un alojamiento")]
    public int IdAlojamiento { get; set; }

    [Required(ErrorMessage = "Seleccione una temporada")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una temporada")]
    public int IdTemporada { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio base debe ser mayor a cero")]
    public decimal PrecioBase { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio adicional no puede ser negativo")]
    public decimal PrecioPersonaAdicional { get; set; }

    public bool Activa { get; set; } = true;

    public static TarifaFormViewModel FromDto(TarifaDto dto) => new()
    {
        IdTarifa = dto.IdTarifa,
        IdAlojamiento = dto.IdAlojamiento,
        IdTemporada = dto.IdTemporada,
        PrecioBase = dto.PrecioBase,
        PrecioPersonaAdicional = dto.PrecioPersonaAdicional,
        Activa = dto.Activa
    };

    public CrearTarifaCommand ToCrearCommand() =>
        new(IdAlojamiento, IdTemporada, PrecioBase, PrecioPersonaAdicional, Activa);

    public ActualizarTarifaCommand ToActualizarCommand() =>
        new(IdTarifa, IdAlojamiento, IdTemporada, PrecioBase, PrecioPersonaAdicional, Activa);
}
