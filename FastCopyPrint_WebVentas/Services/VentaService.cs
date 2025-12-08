using FastCopyPrint_Ventas.Models;
using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;

namespace FastCopyPrint_WebVentas.Services;

public class VentaService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task<int> ProcesarVentaAsync(int clienteId, string direccionEnvio, string nombreMetodoPago)
    {
        await using var context = await dbFactory.CreateDbContextAsync();

        var carrito = await context.Carritos
            .Include(c => c.Items)
            .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);

        if (carrito == null || !carrito.Items.Any())
            throw new InvalidOperationException("El carrito está vacío.");

        var metodoPagoDb = await context.MetodosPago
            .FirstOrDefaultAsync(m => m.Nombre == nombreMetodoPago);

        if (metodoPagoDb == null)
            throw new ArgumentException("Método de pago no válido.");

        var cliente = await context.Clientes.FindAsync(clienteId);
        if (cliente != null)
        {
            cliente.DireccionEnvio = direccionEnvio;
        }

        var nuevaVenta = new Venta
        {
            ClienteId = clienteId,
            FechaVenta = DateTime.UtcNow,
            TotalVenta = carrito.Items.Sum(i => i.Cantidad * i.Producto.Precio),
            EstadoVenta = "Completada",
            NumeroFactura = GenerarNumeroFactura(),
            MetodoPagoId = metodoPagoDb.MetodoPagoId,
            VendedorId = null 
        };

        context.Ventas.Add(nuevaVenta);

        foreach (var item in carrito.Items)
        {
            if (item.Producto.Stock < item.Cantidad)
                throw new InvalidOperationException($"Stock insuficiente para el producto: {item.Producto.Nombre}");

            var detalle = new VentaDetalle
            {
                Venta = nuevaVenta,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.Producto.Precio
            };
            context.VentasDetalles.Add(detalle);

            item.Producto.Stock -= item.Cantidad;
        }

        context.CarritoItems.RemoveRange(carrito.Items);

        await context.SaveChangesAsync();

        return nuevaVenta.VentaId;
    }

    private string GenerarNumeroFactura()
    {
        return $"FAC-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
    }
}