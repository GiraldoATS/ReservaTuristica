using System.ComponentModel.DataAnnotations;

namespace FondoXYZ.Entities.ViewModels
{
    public class ReservaCreateViewModel
    {
        public int IdAlojamiento { get; set; }

        public string NombreAlojamiento
        { get; set; } = string.Empty;

        public int CapacidadMaxima
        { get; set; }

        [Required]
        public DateTime FechaInicio
        { get; set; }

        [Required]
        public DateTime FechaFin
        { get; set; }

        [Required]
        public int NumeroPersonas
        { get; set; }

        public decimal Total
        { get; set; }
    }
}