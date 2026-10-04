using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesaDeAyuda.Migrations
{
    /// <inheritdoc />
    public partial class AddMailAndPhoneToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MailCliente",
                table: "Caso",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroTelefonoCliente",
                table: "Caso",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Caso",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MailCliente", "NumeroTelefonoCliente" },
                values: new object[] { "cliente5001@example.com", "+54 9 11 5001" });

            migrationBuilder.UpdateData(
                table: "Caso",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MailCliente", "NumeroTelefonoCliente" },
                values: new object[] { "cliente5002@example.com", "+54 9 11 5002" });

            migrationBuilder.UpdateData(
                table: "Caso",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MailCliente", "NumeroTelefonoCliente" },
                values: new object[] { "cliente5003@example.com", "+54 9 11 5003" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailCliente",
                table: "Caso");

            migrationBuilder.DropColumn(
                name: "NumeroTelefonoCliente",
                table: "Caso");
        }
    }
}
