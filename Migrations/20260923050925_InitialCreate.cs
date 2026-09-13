using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MesaDeAyuda.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoCaso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoCaso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoCasoInstancia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoCasoInstancia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCaso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    NumeroMaximaIteracion = table.Column<int>(type: "integer", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCaso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoTarea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTarea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Especialista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Legajo = table.Column<int>(type: "integer", nullable: false),
                    Cuit = table.Column<int>(type: "integer", nullable: false),
                    NombreApellido = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SectorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Especialista_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoInstancia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaHoraBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SectorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoInstancia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoInstancia_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Caso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroCaso = table.Column<int>(type: "integer", nullable: false),
                    FechaHoraIngreso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaHoraFinCaso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaHoraCaducidad = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NumeroIteracion = table.Column<int>(type: "integer", nullable: false),
                    NumeroCliente = table.Column<int>(type: "integer", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    TipoCasoId = table.Column<int>(type: "integer", nullable: false),
                    EstadoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caso_EstadoCaso_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadoCaso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Caso_TipoCaso_TipoCasoId",
                        column: x => x.TipoCasoId,
                        principalTable: "TipoCaso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoCasoIteracion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroDeIteracion = table.Column<int>(type: "integer", nullable: false),
                    CoeficienteReduccionTipo = table.Column<int>(type: "integer", nullable: false),
                    TipoCasoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCasoIteracion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoCasoIteracion_TipoCaso_TipoCasoId",
                        column: x => x.TipoCasoId,
                        principalTable: "TipoCaso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoCasoTipoInstancia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    MinutosMaximaResolucion = table.Column<int>(type: "integer", nullable: false),
                    TipoValidacionCierre = table.Column<int>(type: "integer", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaHoraVerificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TipoCasoId = table.Column<int>(type: "integer", nullable: false),
                    TipoInstanciaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCasoTipoInstancia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoCasoTipoInstancia_TipoCaso_TipoCasoId",
                        column: x => x.TipoCasoId,
                        principalTable: "TipoCaso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TipoCasoTipoInstancia_TipoInstancia_TipoInstanciaId",
                        column: x => x.TipoInstanciaId,
                        principalTable: "TipoInstancia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasoInstancia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrdenCasoInstancia = table.Column<int>(type: "integer", nullable: false),
                    FechaHoraInicioReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaHoraFinReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaHoraInicioPlanificada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaHoraFinPlanificada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    CasoId = table.Column<int>(type: "integer", nullable: false),
                    EspecialistaId = table.Column<int>(type: "integer", nullable: true),
                    TipoInstanciaId = table.Column<int>(type: "integer", nullable: false),
                    EstadoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasoInstancia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasoInstancia_Caso_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Caso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CasoInstancia_Especialista_EspecialistaId",
                        column: x => x.EspecialistaId,
                        principalTable: "Especialista",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CasoInstancia_EstadoCasoInstancia_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadoCasoInstancia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CasoInstancia_TipoInstancia_TipoInstanciaId",
                        column: x => x.TipoInstanciaId,
                        principalTable: "TipoInstancia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasoInstanciaTarea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaHoraInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaHoraFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    CasoInstanciaId = table.Column<int>(type: "integer", nullable: false),
                    TipoTareaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasoInstanciaTarea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasoInstanciaTarea_CasoInstancia_CasoInstanciaId",
                        column: x => x.CasoInstanciaId,
                        principalTable: "CasoInstancia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CasoInstanciaTarea_TipoTarea_TipoTareaId",
                        column: x => x.TipoTareaId,
                        principalTable: "TipoTarea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EstadoCaso",
                columns: new[] { "Id", "FechaHoraBaja", "Nombre" },
                values: new object[,]
                {
                    { 1, null, "Tomado" },
                    { 2, null, "Disponible" },
                    { 3, null, "Cerrado" },
                    { 4, null, "Terminado Sin Éxito en la Iteración" },
                    { 5, null, "Terminado Sin Solución" }
                });

            migrationBuilder.InsertData(
                table: "EstadoCasoInstancia",
                columns: new[] { "Id", "FechaHoraBaja", "Nombre" },
                values: new object[,]
                {
                    { 1, null, "Asignada" },
                    { 2, null, "Resuelto" },
                    { 3, null, "Cancelada" },
                    { 4, null, "Sin Resolver" },
                    { 5, null, "A Asignar" },
                    { 6, null, "Sin Asignar" },
                    { 7, null, "Caducada" }
                });

            migrationBuilder.InsertData(
                table: "Sector",
                columns: new[] { "Id", "Descripcion", "FechaHoraBaja", "Nombre" },
                values: new object[,]
                {
                    { 1, "Atención primaria vía telefónica.", null, "Atención Telefónica" },
                    { 2, "Soporte técnico de segundo nivel.", null, "Soporte Técnico" },
                    { 3, "Especialistas en infraestructura de red.", null, "Redes" }
                });

            migrationBuilder.InsertData(
                table: "TipoCaso",
                columns: new[] { "Id", "FechaHoraBaja", "Nombre", "NumeroMaximaIteracion" },
                values: new object[] { 1, null, "Falla de Conexión", 3 });

            migrationBuilder.InsertData(
                table: "TipoTarea",
                columns: new[] { "Id", "Descripcion", "FechaHoraBaja", "Nombre" },
                values: new object[,]
                {
                    { 1, "Contacto telefónico realizado hacia el cliente.", null, "Llamada Saliente" },
                    { 2, "Revisión técnica del problema reportado.", null, "Diagnóstico Técnico" },
                    { 3, "Derivación del caso a otro sector o especialista.", null, "Derivación Interna" }
                });

            migrationBuilder.InsertData(
                table: "Caso",
                columns: new[] { "Id", "EstadoId", "FechaHoraCaducidad", "FechaHoraFinCaso", "FechaHoraIngreso", "NumeroCaso", "NumeroCliente", "NumeroIteracion", "Observaciones", "TipoCasoId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 9, 16, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), 1001, 5001, 1, "Cliente reporta caída intermitente de conexión.", 1 },
                    { 2, 1, new DateTime(2026, 9, 16, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), 1002, 5002, 1, "Reclamo por corte total del servicio.", 1 },
                    { 3, 2, new DateTime(2026, 9, 16, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), 1003, 5003, 1, "Consulta por lentitud de conexión.", 1 }
                });

            migrationBuilder.InsertData(
                table: "Especialista",
                columns: new[] { "Id", "Cuit", "FechaHoraBaja", "Legajo", "NombreApellido", "SectorId" },
                values: new object[,]
                {
                    { 1, 201111116, null, 1001, "Juan Pérez", 1 },
                    { 2, 272222223, null, 1002, "Ana Gómez", 2 },
                    { 3, 203333334, null, 1003, "Carlos López", 3 }
                });

            migrationBuilder.InsertData(
                table: "TipoCasoIteracion",
                columns: new[] { "Id", "CoeficienteReduccionTipo", "NumeroDeIteracion", "TipoCasoId" },
                values: new object[,]
                {
                    { 1, 75, 2, 1 },
                    { 2, 50, 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "TipoInstancia",
                columns: new[] { "Id", "FechaHoraBaja", "Nombre", "SectorId" },
                values: new object[,]
                {
                    { 1, null, "Atención Telefónica", 1 },
                    { 2, null, "Soporte Técnico", 2 },
                    { 3, null, "Redes", 3 }
                });

            migrationBuilder.InsertData(
                table: "CasoInstancia",
                columns: new[] { "Id", "CasoId", "EspecialistaId", "EstadoId", "FechaHoraFinPlanificada", "FechaHoraFinReal", "FechaHoraInicioPlanificada", "FechaHoraInicioReal", "Observaciones", "OrdenCasoInstancia", "TipoInstanciaId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), "", 1, 1 },
                    { 2, 1, null, 6, new DateTime(2026, 9, 12, 12, 0, 0, 0, DateTimeKind.Utc), null, null, null, "", 2, 2 },
                    { 3, 1, null, 6, new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Utc), null, null, null, "", 3, 3 },
                    { 4, 2, 1, 4, new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), "No se logró restablecer el servicio en el primer contacto.", 1, 1 },
                    { 5, 2, 2, 4, new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), "Se descarta falla en el domicilio del cliente.", 2, 2 },
                    { 6, 2, 3, 1, new DateTime(2026, 9, 16, 12, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Utc), "", 3, 3 },
                    { 7, 3, null, 5, new DateTime(2026, 9, 11, 12, 0, 0, 0, DateTimeKind.Utc), null, null, null, "", 1, 1 },
                    { 8, 3, null, 6, new DateTime(2026, 9, 12, 12, 0, 0, 0, DateTimeKind.Utc), null, null, null, "", 2, 2 },
                    { 9, 3, null, 6, new DateTime(2026, 9, 13, 12, 0, 0, 0, DateTimeKind.Utc), null, null, null, "", 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "TipoCasoTipoInstancia",
                columns: new[] { "Id", "FechaAlta", "FechaBaja", "FechaHoraVerificacion", "MinutosMaximaResolucion", "Orden", "TipoCasoId", "TipoInstanciaId", "TipoValidacionCierre" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, null, 1440, 1, 1, 1, 0 },
                    { 2, new DateTime(2025, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, null, 2880, 2, 1, 2, 2 },
                    { 3, new DateTime(2025, 9, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, null, 4320, 3, 1, 3, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caso_EstadoId",
                table: "Caso",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Caso_TipoCasoId",
                table: "Caso",
                column: "TipoCasoId");

            migrationBuilder.CreateIndex(
                name: "IX_CasoInstancia_CasoId",
                table: "CasoInstancia",
                column: "CasoId");

            migrationBuilder.CreateIndex(
                name: "IX_CasoInstancia_EspecialistaId",
                table: "CasoInstancia",
                column: "EspecialistaId");

            migrationBuilder.CreateIndex(
                name: "IX_CasoInstancia_EstadoId",
                table: "CasoInstancia",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_CasoInstancia_TipoInstanciaId",
                table: "CasoInstancia",
                column: "TipoInstanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_CasoInstanciaTarea_CasoInstanciaId",
                table: "CasoInstanciaTarea",
                column: "CasoInstanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_CasoInstanciaTarea_TipoTareaId",
                table: "CasoInstanciaTarea",
                column: "TipoTareaId");

            migrationBuilder.CreateIndex(
                name: "IX_Especialista_SectorId",
                table: "Especialista",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCasoIteracion_TipoCasoId",
                table: "TipoCasoIteracion",
                column: "TipoCasoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCasoTipoInstancia_TipoCasoId",
                table: "TipoCasoTipoInstancia",
                column: "TipoCasoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCasoTipoInstancia_TipoInstanciaId",
                table: "TipoCasoTipoInstancia",
                column: "TipoInstanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoInstancia_SectorId",
                table: "TipoInstancia",
                column: "SectorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CasoInstanciaTarea");

            migrationBuilder.DropTable(
                name: "TipoCasoIteracion");

            migrationBuilder.DropTable(
                name: "TipoCasoTipoInstancia");

            migrationBuilder.DropTable(
                name: "CasoInstancia");

            migrationBuilder.DropTable(
                name: "TipoTarea");

            migrationBuilder.DropTable(
                name: "Caso");

            migrationBuilder.DropTable(
                name: "Especialista");

            migrationBuilder.DropTable(
                name: "EstadoCasoInstancia");

            migrationBuilder.DropTable(
                name: "TipoInstancia");

            migrationBuilder.DropTable(
                name: "EstadoCaso");

            migrationBuilder.DropTable(
                name: "TipoCaso");

            migrationBuilder.DropTable(
                name: "Sector");
        }
    }
}
