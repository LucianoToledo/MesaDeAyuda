using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MesaDeAyuda.Migrations
{
    /// <inheritdoc />
    public partial class AddTableTipoValidacionCierre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoValidacionCierre",
                table: "TipoCasoTipoInstancia",
                newName: "TipoValidacionCierreId");

            migrationBuilder.CreateTable(
                name: "TipoValidacionCierre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoValidacionCierre", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "TipoCasoTipoInstancia",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoValidacionCierreId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TipoCasoTipoInstancia",
                keyColumn: "Id",
                keyValue: 2,
                column: "TipoValidacionCierreId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TipoCasoTipoInstancia",
                keyColumn: "Id",
                keyValue: 3,
                column: "TipoValidacionCierreId",
                value: 2);

            migrationBuilder.InsertData(
                table: "TipoValidacionCierre",
                columns: new[] { "Id", "FechaHoraBaja", "Nombre" },
                values: new object[,]
                {
                    { 1, null, "Simple" },
                    { 2, null, "PorObservaciones" },
                    { 3, null, "PorTareaRegistrada" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoCasoTipoInstancia_TipoValidacionCierreId",
                table: "TipoCasoTipoInstancia",
                column: "TipoValidacionCierreId");

            migrationBuilder.AddForeignKey(
                name: "FK_TipoCasoTipoInstancia_TipoValidacionCierre_TipoValidacionCi~",
                table: "TipoCasoTipoInstancia",
                column: "TipoValidacionCierreId",
                principalTable: "TipoValidacionCierre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TipoCasoTipoInstancia_TipoValidacionCierre_TipoValidacionCi~",
                table: "TipoCasoTipoInstancia");

            migrationBuilder.DropTable(
                name: "TipoValidacionCierre");

            migrationBuilder.DropIndex(
                name: "IX_TipoCasoTipoInstancia_TipoValidacionCierreId",
                table: "TipoCasoTipoInstancia");

            migrationBuilder.RenameColumn(
                name: "TipoValidacionCierreId",
                table: "TipoCasoTipoInstancia",
                newName: "TipoValidacionCierre");

            migrationBuilder.UpdateData(
                table: "TipoCasoTipoInstancia",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoValidacionCierre",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TipoCasoTipoInstancia",
                keyColumn: "Id",
                keyValue: 2,
                column: "TipoValidacionCierre",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TipoCasoTipoInstancia",
                keyColumn: "Id",
                keyValue: 3,
                column: "TipoValidacionCierre",
                value: 1);
        }
    }
}
