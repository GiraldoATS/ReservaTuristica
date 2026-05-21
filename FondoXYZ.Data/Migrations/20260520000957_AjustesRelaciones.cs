using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FondoXYZ.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustesRelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disponibilidades",
                columns: table => new
                {
                    IdAlojamiento = table.Column<int>(type: "int", nullable: false),
                    Sede = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alojamiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_IdAlojamiento",
                table: "Tarifas",
                column: "IdAlojamiento");

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_IdTemporada",
                table: "Tarifas",
                column: "IdTemporada");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarifas_Alojamientos_IdAlojamiento",
                table: "Tarifas",
                column: "IdAlojamiento",
                principalTable: "Alojamientos",
                principalColumn: "IdAlojamiento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarifas_Temporadas_IdTemporada",
                table: "Tarifas",
                column: "IdTemporada",
                principalTable: "Temporadas",
                principalColumn: "IdTemporada",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tarifas_Alojamientos_IdAlojamiento",
                table: "Tarifas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarifas_Temporadas_IdTemporada",
                table: "Tarifas");

            migrationBuilder.DropTable(
                name: "Disponibilidades");

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_IdAlojamiento",
                table: "Tarifas");

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_IdTemporada",
                table: "Tarifas");
        }
    }
}
