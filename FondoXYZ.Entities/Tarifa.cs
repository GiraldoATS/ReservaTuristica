using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FondoXYZ.Entities
{
    public class Tarifa
    {
        [Key]
        public int IdTarifa { get; set; }

        public int IdAlojamiento { get; set; }

        public int IdTemporada { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioBase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioPersonaAdicional { get; set; }

        public bool Estado { get; set; }

        public Alojamiento? Alojamiento { get; set; }

        public Temporada? Temporada { get; set; }
    }
}