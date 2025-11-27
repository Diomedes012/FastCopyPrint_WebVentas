using FastCopyPrint_Ventas.Models;
using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace FastCopyPrint_WebVentas.Services;

public class CategoriasService(IDbContextFactory<ApplicationDbContext> factory)
{
    public async Task<bool> Guardar(Categoria categoria)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        if (!await Existe(categoria.CategoriaId))
        {
            return await Insertar(categoria);
        }
        else
        {
            return await Modificar(categoria);
        }
    }

    private async Task<bool> Existe(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Categorias.AnyAsync(c => c.CategoriaId == id);
    }

    private async Task<bool> Insertar(Categoria categoria)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        contexto.Categorias.Add(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }
    private async Task<bool> Modificar(Categoria categoria)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        contexto.Categorias.Update(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Categoria?> Buscar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoriaId == id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await factory.CreateDbContextAsync();

        // Regla de negocio: No borrar categoría si tiene productos
        bool tieneHijos = await contexto.Productos.AnyAsync(p => p.CategoriaId == id);
        if (tieneHijos) return false;

        var categoria = await contexto.Categorias.FindAsync(id);
        if (categoria == null) return false;

        contexto.Categorias.Remove(categoria);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Categoria>> Listar(Expression<Func<Categoria, bool>> criterio)
    {
        await using var contexto = await factory.CreateDbContextAsync();
        return await contexto.Categorias
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
