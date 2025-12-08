using FastCopyPrint_Ventas.Models;
using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;

namespace FastCopyPrint_WebVentas.Services;

public class CarritoService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public event Action? OnCambio;
    private void NotificarCambio() => OnCambio?.Invoke();


    // Obtener el carrito de compras completo con sus items y productos
    public async Task<List<Carrito>> ObtenerCarritoAsync()
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        return await Context.Carritos
            .Include(c => c.Items)
            .ThenInclude(i => i.Producto)
            .ToListAsync();
    }

    // Agregar un item al carrito de compras
    public async Task<bool> AgregarItemAlCarrito(int carritoId, int productoId, int cantidad)
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        var carritoItemExistente = await Context.CarritoItems
            .FirstOrDefaultAsync(ci => ci.CarritoId == carritoId && ci.ProductoId == productoId);

        if (carritoItemExistente != null)
        {
            carritoItemExistente.Cantidad += cantidad; // Actualizar cantidad si el item ya existe
        }
        else
        {
            var existeCarrito = await Context.Carritos.AnyAsync(c => c.CarritoId == carritoId);
            if (!existeCarrito)
            {
                return false; // Carrito no encontrado
            }
            Context.CarritoItems.Add(new CarritoItem
            {
                CarritoId = carritoId,
                ProductoId = productoId,
                Cantidad = cantidad
            });
        }
        await Context.SaveChangesAsync();
        NotificarCambio();
        return true;
    }

    // Eliminar un item del carrito de compras
    public async Task<bool> EliminarItemDelCarrito(int carritoId, int productoId)
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        var filasAfectadas = await Context.CarritoItems
            .Where(ci => ci.CarritoId == carritoId && ci.ProductoId == productoId)
            .ExecuteDeleteAsync();
        NotificarCambio();
        return filasAfectadas > 0;
    }

    // Disminuir la cantidad de un item en el carrito de compras
    public async Task<bool> DesminuirItemDelCarrito(int carritoId, int productoId, int cantidad)
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        var carritoItem = await Context.CarritoItems
            .FirstOrDefaultAsync(ci => ci.CarritoId == carritoId && ci.ProductoId == productoId);
        if (carritoItem == null)
        {
            return false; // Item no encontrado
        }
        carritoItem.Cantidad -= cantidad;
        if (carritoItem.Cantidad <= 0)
        {
            Context.CarritoItems.Remove(carritoItem); // Eliminar item si la cantidad es 0 o menor
        }
        await Context.SaveChangesAsync();
        NotificarCambio();
        return true;
    }

    // Limpiar el carrito de compras totalmente
    public async Task<bool> LimpiarCarritodeCompras(int carritoId)
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        await Context.CarritoItems
            .Where(ci => ci.CarritoId == carritoId)
            .ExecuteDeleteAsync();
        NotificarCambio();
        return true;
    }

    // Obtener el carrito de compras por el ID del cliente
    public async Task<Carrito?> ObtenerCarritoPorClienteIdAsync(int clienteId)
    {
        await using var Context = await dbFactory.CreateDbContextAsync();

        var carrito = await Context.Carritos
            .Include(c => c.Items)
            .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);

        if (carrito == null)
        {
            bool existeCliente = await Context.Clientes.AnyAsync(c => c.ClienteId == clienteId);

            if (!existeCliente)
            {
                return null;
            }

            carrito = new Carrito
            {
                ClienteId = clienteId,
                FechaCreacion = DateTime.UtcNow,
            };
            Context.Carritos.Add(carrito);
            await Context.SaveChangesAsync();
        }

        return carrito;
    }

    // Calcular el total del carrito de compras
    public async Task<decimal> CalcularTotalCarritoAsync(int carritoId)
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        return await Context.CarritoItems
            .Where(ci => ci.CarritoId == carritoId)
            .SumAsync(ci => ci.Producto.Precio * ci.Cantidad);
    }


}
