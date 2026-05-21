namespace FondoXYZ.Entities.DTOs
{
    public class DashboardDto
    {
        public int TotalReservas { get; set; }

        public decimal IngresosTotales { get; set; }

        public int TotalAlojamientos { get; set; }

        public int TarifasActivas { get; set; }

        public List<Reserva> UltimasReservas
        { get; set; } = new();
    }
}