namespace FondoXYZ.Entities.DTOs
{
    public class CalcularTarifaRequest
    {
        public int IdAlojamiento { get; set; }

        public int NumeroPersonas { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }
    }
}