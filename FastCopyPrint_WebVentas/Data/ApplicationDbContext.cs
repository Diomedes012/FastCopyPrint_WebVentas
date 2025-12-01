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
            new IdentityRole { Id = "1", Name = "Encargado", NormalizedName = "ENCARGADO" },
            new IdentityRole { Id = "2", Name = "Cliente", NormalizedName = "CLIENTE" }
        );

        builder.Entity<MetodoPago>().HasData(
            new MetodoPago { MetodoPagoId = 1, Nombre = "Efectivo", Descripcion = "Pago contra entrega" },
            new MetodoPago { MetodoPagoId = 2, Nombre = "Tarjeta", Descripcion = "Crédito o Débito" }
        );
    }

}
