using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FondoXYZ.Entities
{
    public class Reserva
    {
        [Key]
        public int IdReserva { get; set; }

        [Required]
        public string IdUsuario { get; set; } = string.Empty;

        [Required]
        public int IdAlojamiento { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public int NumeroPersonas { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public string Estado { get; set; } = "ACTIVA";

        public DateTime FechaReserva { get; set; }
            = DateTime.Now;

        // =========================
        // RELACIONES
        // =========================

        public Alojamiento? Alojamiento { get; set; }

        public ApplicationUser? Usuario { get; set; }
    }
}