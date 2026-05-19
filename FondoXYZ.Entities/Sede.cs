using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;


namespace FondoXYZ.Entities
{
    public class Sede
    {
        [Key]
        public int IdSede { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Ciudad { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public int CapacidadTotal { get; set; }

        public bool Estado { get; set; }

        public ICollection<Alojamiento> Alojamientos
        { get; set; } = new List<Alojamiento>();
    }
}