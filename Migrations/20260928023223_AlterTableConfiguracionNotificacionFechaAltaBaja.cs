using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesaDeAyuda.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableConfiguracionNotificacionFechaAltaBaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                table: "ConfiguracionNotificacion",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "ConfiguracionNotificacion",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ConfiguracionNotificacion",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaAlta", "FechaBaja" },
                values: new object[] { new DateTime(2025, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaAlta",
                table: "ConfiguracionNotificacion");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "ConfiguracionNotificacion");
        }
    }
}
