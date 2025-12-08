using FastCopyPrint_Ventas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FastCopyPrint_WebVentas.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Venta> Ventas { get; set; }
    public DbSet<VentaDetalle> VentasDetalles { get; set; }
    public DbSet<MetodoPago> MetodosPago { get; set; }

    public DbSet<Carrito> Carritos { get; set; }
    public DbSet<CarritoItem> CarritoItems { get; set; }
    public DbSet<ListaDeseo> ListasDeseos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var propiedad in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            propiedad.SetColumnType("decimal(18, 2)");
        }

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Encargado", NormalizedName = "ENCARGADO", ConcurrencyStamp = "1_stamp_estatico" },
            new IdentityRole { Id = "2", Name = "Cliente", NormalizedName = "CLIENTE", ConcurrencyStamp = "2_stamp_estatico" }
        );

        builder.Entity<MetodoPago>().HasData(
            new MetodoPago { MetodoPagoId = 1, Nombre = "Efectivo", Descripcion = "Pago contra entrega" },
            new MetodoPago { MetodoPagoId = 2, Nombre = "Tarjeta", Descripcion = "Crédito o Débito" }
        );

        builder.Entity<Categoria>().HasData(
        new Categoria { CategoriaId = 1, Nombre = "Papelería", Descripcion = "Papel, cuadernos y sobres" },
        new Categoria { CategoriaId = 2, Nombre = "Escritura", Descripcion = "Bolígrafos, lápices y marcadores" },
        new Categoria { CategoriaId = 3, Nombre = "Oficina", Descripcion = "Archivadores, grapadoras y clips" },
        new Categoria { CategoriaId = 4, Nombre = "Tecnología", Descripcion = "Memorias USB, cables y accesorios" }
        );

        builder.Entity<Producto>().HasData(
            new Producto
            {
                ProductoId = 1,
                Nombre = "Resma Papel Bond 8.5x11",
                Descripcion = "Papel ultra blanco 75g, 500 hojas.",
                Precio = 450.00m,
                Stock = 100,
                CategoriaId = 1,
                EstaActivo = true,
                ImagenUrl = "images/ResmaPapel.jpg"
            },
            new Producto
            {
                ProductoId = 2,
                Nombre = "Caja Bolígrafos Azul",
                Descripcion = "Caja de 12 unidades, punta media.",
                Precio = 350.00m,
                Stock = 50,
                CategoriaId = 2,
                EstaActivo = true,
                ImagenUrl = "images/CajaLapiceros.jpg"
            },
            new Producto
            {
                ProductoId = 3,
                Nombre = "Folder Manila 8.5x11",
                Descripcion = "Paquete de 10 folders color crema.",
                Precio = 80.00m,
                Stock = 200,
                CategoriaId = 3,
                EstaActivo = true,
                ImagenUrl = "images/Folders.jfif"

            },
            new Producto
            {
                ProductoId = 4,
                Nombre = "Memoria USB 32GB",
                Descripcion = "USB 3.0 Kingston DataTraveler.",
                Precio = 450.00m,
                Stock = 15,
                CategoriaId = 4,
                EstaActivo = true,
                ImagenUrl = "images/USB.webp"
            },
            new Producto
            {
                ProductoId = 5,
                Nombre = "Marcador Permanente Negro",
                Descripcion = "Punta gruesa, secado rápido.",
                Precio = 65.00m,
                Stock = 4,
                CategoriaId = 2,
                EstaActivo = true,
                ImagenUrl = "Marcador.webp"
            }
        );

    }

}
