using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FondoXYZ.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EliminarTablasDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculoTarifas");

            migrationBuilder.DropTable(
                name: "Disponibilidades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculoTarifas",
                columns: table => new
                {
                    IdTemporada = table.Column<int>(type: "int", nullable: false),
                    NumeroNoches = table.Column<int>(type: "int", nullable: false),
                    PersonasAdicionales = table.Column<int>(type: "int", nullable: false),
                    PrecioBase = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioPersonaAdicional = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPagar = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Disponibilidades",
                columns: table => new
                {
                    Alojamiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "int", nullable: false),
                    IdAlojamiento = table.Column<int>(type: "int", nullable: false),
                    Sede = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }
    }
}
