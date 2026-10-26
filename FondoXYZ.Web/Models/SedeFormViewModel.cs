using System.ComponentModel.DataAnnotations;
using FondoXYZ.Application.DTOs;

namespace FondoXYZ.Web.Models;

public class SedeFormViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    public CrearSedeCommand ToCommand() => new(Nombre);
}
