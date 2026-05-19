using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace FondoXYZ.Entities
{
    public class Alojamiento
    {
        [Key]
        public int IdAlojamiento { get; set; }

        public int IdSede { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;

        public int CapacidadMaxima { get; set; }

        public int CantidadHabitaciones { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public Sede? Sede { get; set; }
        public ICollection<Reserva> Reservas
        { get; set; } = new List<Reserva>();
    }
}