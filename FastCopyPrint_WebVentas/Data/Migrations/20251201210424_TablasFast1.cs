using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastCopyPrint_WebVentas.Migrations
{
    /// <inheritdoc />
    public partial class TablasFast1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "7bceef36-8c10-4d0e-9f2f-4ca9b52dd83c", "Encargado", "ENCARGADO" },
                    { "2", "20ef5edc-ce51-4d34-9b09-5b1bbd2a2b75", "Cliente", "CLIENTE" }
                });

            migrationBuilder.InsertData(
                table: "MetodosPago",
                columns: new[] { "MetodoPagoId", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pago contra entrega", "Efectivo" },
                    { 2, "Crédito o Débito", "Tarjeta" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "MetodosPago",
                keyColumn: "MetodoPagoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MetodosPago",
                keyColumn: "MetodoPagoId",
                keyValue: 2);
        }
    }
}
