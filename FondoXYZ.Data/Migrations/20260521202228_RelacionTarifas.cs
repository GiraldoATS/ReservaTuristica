using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FondoXYZ.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionTarifas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculoTarifas",
                columns: table => new
                {
                    IdTemporada = table.Column<int>(type: "int", nullable: false),
                    PrecioBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioPersonaAdicional = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumeroNoches = table.Column<int>(type: "int", nullable: false),
                    PersonasAdicionales = table.Column<int>(type: "int", nullable: false),
                    TotalPagar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculoTarifas");
        }
    }
}
