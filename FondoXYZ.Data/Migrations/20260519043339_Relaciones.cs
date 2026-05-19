using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FondoXYZ.Data.Migrations
{
    /// <inheritdoc />
    public partial class Relaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alojamientos_Sedes_SedeIdSede",
                table: "Alojamientos");

            migrationBuilder.DropIndex(
                name: "IX_Alojamientos_SedeIdSede",
                table: "Alojamientos");

            migrationBuilder.DropColumn(
                name: "SedeIdSede",
                table: "Alojamientos");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdAlojamiento",
                table: "Reservas",
                column: "IdAlojamiento");

            migrationBuilder.CreateIndex(
                name: "IX_Alojamientos_IdSede",
                table: "Alojamientos",
                column: "IdSede");

            migrationBuilder.AddForeignKey(
                name: "FK_Alojamientos_Sedes_IdSede",
                table: "Alojamientos",
                column: "IdSede",
                principalTable: "Sedes",
                principalColumn: "IdSede",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Alojamientos_IdAlojamiento",
                table: "Reservas",
                column: "IdAlojamiento",
                principalTable: "Alojamientos",
                principalColumn: "IdAlojamiento",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alojamientos_Sedes_IdSede",
                table: "Alojamientos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Alojamientos_IdAlojamiento",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_IdAlojamiento",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Alojamientos_IdSede",
                table: "Alojamientos");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "Reservas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SedeIdSede",
                table: "Alojamientos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alojamientos_SedeIdSede",
                table: "Alojamientos",
                column: "SedeIdSede");

            migrationBuilder.AddForeignKey(
                name: "FK_Alojamientos_Sedes_SedeIdSede",
                table: "Alojamientos",
                column: "SedeIdSede",
                principalTable: "Sedes",
                principalColumn: "IdSede");
        }
    }
}
