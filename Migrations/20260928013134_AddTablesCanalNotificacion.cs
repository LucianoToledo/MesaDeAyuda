using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MesaDeAyuda.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesCanalNotificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CanalNotificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanalNotificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionNotificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanalHabilitadoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionNotificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionNotificacion_CanalNotificacion_CanalHabilitado~",
                        column: x => x.CanalHabilitadoId,
                        principalTable: "CanalNotificacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CanalNotificacion",
                columns: new[] { "Id", "FechaHoraBaja", "Nombre" },
                values: new object[,]
                {
                    { 1, null, "Email" },
                    { 2, null, "Sms" }
                });

            migrationBuilder.InsertData(
                table: "ConfiguracionNotificacion",
                columns: new[] { "Id", "CanalHabilitadoId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionNotificacion_CanalHabilitadoId",
                table: "ConfiguracionNotificacion",
                column: "CanalHabilitadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionNotificacion");

            migrationBuilder.DropTable(
                name: "CanalNotificacion");
        }
    }
}
