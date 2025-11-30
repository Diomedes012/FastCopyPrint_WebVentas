using FastCopyPrint_Ventas.Models;
using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;

namespace FastCopyPrint_WebVentas.Services;

public class DashboardService(IDbContextFactory<ApplicationDbContext> factory)
{
    public async Task<ResumenVentas> ObtenerMetricas()
    {
        await using var context = await factory.CreateDbContextAsync();
        var data = new ResumenVentas(); 

        var hoy = DateTime.Today;
        var primerDiaMes = new DateTime(hoy.Year, hoy.Month, 1);

        data.VentasHoy = await context.Ventas
            .Where(v => v.FechaVenta >= hoy)
            .SumAsync(v => v.TotalVenta);

        data.TransaccionesHoy = await context.Ventas
            .CountAsync(v => v.FechaVenta >= hoy);

        data.VentasMes = await context.Ventas
            .Where(v => v.FechaVenta >= primerDiaMes)
            .SumAsync(v => v.TotalVenta);

        data.ProductosBajoStock = await context.Productos
            .CountAsync(p => p.Stock <= 5 && p.EstaActivo);

        var ventasSemana = await context.Ventas
            .Where(v => v.FechaVenta >= hoy.AddDays(-6))
            .GroupBy(v => v.FechaVenta.Date)
            .Select(g => new { Fecha = g.Key, Total = g.Sum(v => v.TotalVenta) })
            .ToListAsync();

        var graficoData = new List<double>();
        var graficoLabels = new List<string>();

        for (int i = 6; i >= 0; i--)
        {
            var dia = hoy.AddDays(-i);
            var ventaDia = ventasSemana.FirstOrDefault(v => v.Fecha == dia);
            graficoData.Add((double)(ventaDia?.Total ?? 0));
            graficoLabels.Add(dia.ToString("dd/MM"));
        }

        data.DatosGrafico = graficoData.ToArray();
        data.EtiquetasGrafico = graficoLabels.ToArray();

        data.UltimasVentas = await context.Ventas
            .Include(v => v.Cliente).ThenInclude(c => c.Usuario)
            .OrderByDescending(v => v.FechaVenta)
            .Take(5)
            .AsNoTracking()
            .ToListAsync();

        return data;
    }
}