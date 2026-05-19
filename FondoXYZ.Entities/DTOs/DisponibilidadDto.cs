namespace FondoXYZ.Entities.DTOs
{
    public class DisponibilidadDto
    {
        public int IdAlojamiento { get; set; }

        public string Sede { get; set; } = string.Empty;

        public string Alojamiento { get; set; } = string.Empty;

        public int CapacidadMaxima { get; set; }

        public string Tipo { get; set; } = string.Empty;
    }
}