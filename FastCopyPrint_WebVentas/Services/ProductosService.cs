using FastCopyPrint_Ventas.Models;
using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastCopyPrint_WebVentas.Services;

public class ProductosService(IDbContextFactory<ApplicationDbContext> factory)
{
    public async Task<bool> Guardar(Producto producto)
    {
        if(!string.IsNullOrEmpty(producto.Nombre) && producto.Nombre.Length > 200) 
        {
            throw new Exception($"El nombre es muy largo ({producto.Nombre.Length} caracteres). El limite es 200.");
        }
        if (!string.IsNullOrEmpty(producto.Descripcion) && producto.Descripcion.Length > 1000)
        {
            throw new Exception($"La descripción es muy larga ({producto.Descripcion.Length} caracteres). El límite es 1000.");
        }
        if (producto.Precio < 0)
        {
            throw new Exception("El precio no puede ser negativo.");
        }
        if (producto.Stock < 0)
        {
            throw new Exception("El stock no puede ser negativo.");
        }

        await using var contexto = await factory.CreateDbContextAsync();

        if (!await Existe(producto.ProductoId))
        {
            return await Insertar(producto);
        }
        else
        {
            return await Modificar(producto);
        }
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Productos.AnyAsync(p => p.ProductoId == id);
    }

    private async Task<bool> Insertar(Producto producto)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        contexto.Productos.Add(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Producto producto)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        contexto.Productos.Update(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Producto?> Buscar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Productos
            .Include(p => p.Categoria) // Incluimos la categoria para mostrar el nombre
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductoId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        //No borrar producto si ya se ha vendido (esta en detalles de venta)
        bool tieneVentas = await contexto.VentasDetalles.AnyAsync(d => d.ProductoId == id);
        if (tieneVentas) return false;

        var producto = await contexto.Productos.FindAsync(id);
        if (producto == null) return false;

        contexto.Productos.Remove(producto);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Producto>> Listar(Expression<Func<Producto, bool>> criterio)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Productos
            .Include(p => p.Categoria)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}