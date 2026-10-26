using System.ComponentModel.DataAnnotations;
using FondoXYZ.Application.DTOs;

namespace FondoXYZ.Web.Models;

public class AlojamientoFormViewModel
{
    public int IdAlojamiento { get; set; }

    [Required(ErrorMessage = "Seleccione una sede")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una sede")]
    public int IdSede { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo es obligatorio")]
    public string Tipo { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "La capacidad debe estar entre 1 y 1000")]
    public int CapacidadMaxima { get; set; }

    [Range(0, 1000, ErrorMessage = "Las habitaciones deben estar entre 0 y 1000")]
    public int CantidadHabitaciones { get; set; }

    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public static AlojamientoFormViewModel FromDto(AlojamientoDto dto) => new()
    {
        IdAlojamiento = dto.IdAlojamiento,
        IdSede = dto.IdSede,
        Nombre = dto.Nombre,
        Tipo = dto.Tipo,
        CapacidadMaxima = dto.CapacidadMaxima,
        CantidadHabitaciones = dto.CantidadHabitaciones,
        Descripcion = dto.Descripcion,
        Activo = dto.Activo
    };

    public CrearAlojamientoCommand ToCrearCommand() =>
        new(IdSede, Nombre, Tipo, CapacidadMaxima, CantidadHabitaciones, Descripcion);

    public ActualizarAlojamientoCommand ToActualizarCommand() =>
        new(IdAlojamiento, IdSede, Nombre, Tipo, CapacidadMaxima, CantidadHabitaciones, Descripcion, Activo);
}
