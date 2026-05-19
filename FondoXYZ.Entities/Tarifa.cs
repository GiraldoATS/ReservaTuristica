using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FondoXYZ.Entities
{
    public class Tarifa
    {
        [Key]
        public int IdTarifa { get; set; }

        public int IdAlojamiento { get; set; }

        public int IdTemporada { get; set; }

        public decimal PrecioBase { get; set; }

        public decimal PrecioPersonaAdicional { get; set; }

        public bool Estado { get; set; }
    }
}