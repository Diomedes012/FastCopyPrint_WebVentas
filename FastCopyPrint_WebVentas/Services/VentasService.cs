using FastCopyPrint_Ventas.Models;
using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastCopyPrint_WebVentas.Services;

public class VentasService(IDbContextFactory<ApplicationDbContext> factory)
{
    public async Task<bool> Guardar(Venta venta)
    {
        if (!await Existe(venta.VentaId))
        {
            return await Insertar(venta);
        }
        else
        {
            return await Modificar(venta);
        }
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Ventas.AnyAsync(v => v.VentaId == id);
    }

    private async Task<bool> Insertar(Venta venta)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        using var transaccion = await contexto.Database.BeginTransactionAsync();

        try
        {
            foreach (var detalle in venta.Detalles)
            {
                var producto = await contexto.Productos.FindAsync(detalle.ProductoId);

                if (producto == null)
                    throw new Exception($"Producto ID {detalle.ProductoId} no encontrado.");

                if (producto.Stock < detalle.Cantidad)
                    throw new Exception($"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}");

                // 1. Descontar Stock
                producto.Stock -= detalle.Cantidad;

                // 2. Fijar precio histórico
                detalle.PrecioUnitario = producto.Precio;
            }

            // 3. Agregar la venta
            contexto.Ventas.Add(venta);
            await contexto.SaveChangesAsync();

            // 4. Confirmar transacción
            await transaccion.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaccion.RollbackAsync();
            throw; 
        }
    }

    private async Task<bool> Modificar(Venta venta)
    {
        //Se deberia Modificar una venta????
        await using var contexto = await factory.CreateDbContextAsync();
        contexto.Ventas.Update(venta);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Venta?> Buscar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Vendedor)
            .Include(v => v.MetodoPago)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.VentaId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        var venta = await contexto.Ventas
            .Include(v => v.Detalles) 
            .FirstOrDefaultAsync(v => v.VentaId == id);

        if (venta == null) return false;

        foreach (var detalle in venta.Detalles)
        {
            var producto = await contexto.Productos.FindAsync(detalle.ProductoId);
            if (producto != null)
            {
                producto.Stock += detalle.Cantidad;
            }
        }
        contexto.Ventas.Remove(venta);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Venta>> Listar(Expression<Func<Venta, bool>> criterio)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Vendedor)
            .Include(v => v.MetodoPago)
            .Where(criterio)
            .OrderByDescending(v => v.FechaVenta)
            .AsNoTracking()
            .ToListAsync();
    }
}