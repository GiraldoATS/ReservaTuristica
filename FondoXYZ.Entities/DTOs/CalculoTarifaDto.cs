namespace FondoXYZ.Entities.DTOs
{
    public class CalculoTarifaDto
    {
        public int IdTemporada { get; set; }

        public decimal PrecioBase { get; set; }

        public decimal PrecioPersonaAdicional { get; set; }

        public int NumeroNoches { get; set; }

        public int PersonasAdicionales { get; set; }

        public decimal TotalPagar { get; set; }
    }
}