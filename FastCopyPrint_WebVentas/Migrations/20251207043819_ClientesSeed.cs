using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FastCopyPrint_WebVentas.Migrations
{
    /// <inheritdoc />
    public partial class ClientesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "CategoriaId", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Papel, cuadernos y sobres", "Papelería" },
                    { 2, "Bolígrafos, lápices y marcadores", "Escritura" },
                    { 3, "Archivadores, grapadoras y clips", "Oficina" },
                    { 4, "Memorias USB, cables y accesorios", "Tecnología" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "ProductoId", "CategoriaId", "Descripcion", "EstaActivo", "ImagenUrl", "Nombre", "Precio", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "Papel ultra blanco 75g, 500 hojas.", true, "images/ResmaPapel.jpg", "Resma Papel Bond 8.5x11", 450.00m, 100 },
                    { 2, 2, "Caja de 12 unidades, punta media.", true, "images/CajaLapiceros.jpg", "Caja Bolígrafos Azul", 350.00m, 50 },
                    { 3, 3, "Paquete de 10 folders color crema.", true, "images/Folders.jfif", "Folder Manila 8.5x11", 80.00m, 200 },
                    { 4, 4, "USB 3.0 Kingston DataTraveler.", true, "images/USB.webp", "Memoria USB 32GB", 450.00m, 15 },
                    { 5, 2, "Punta gruesa, secado rápido.", true, "Marcador.webp", "Marcador Permanente Negro", 65.00m, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "ProductoId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "CategoriaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "CategoriaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "CategoriaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "CategoriaId",
                keyValue: 4);
        }
    }
}
