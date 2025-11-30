using FastCopyPrint_Ventas.Models;
using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;

namespace FastCopyPrint_WebVentas.Services;

public class CatalogoService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task<List<Producto>> ObtenerProductosCatalogoAsync()
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        return await Context.Productos
            .Where(p => p.EstaActivo && p.Stock > 0)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerProductoPorIdAsync(int productoId)
    {
        await using var Context = await dbFactory.CreateDbContextAsync();
        return await Context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.ProductoId == productoId);
    }


}
