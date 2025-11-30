using FastCopyPrint_Ventas.Models;

namespace FastCopyPrint_Ventas.Models;

public class ResumenVentas
{
    public decimal VentasHoy { get; set; }
    public int TransaccionesHoy { get; set; }
    public decimal VentasMes { get; set; }
    public int ProductosBajoStock { get; set; }

    public double[] DatosGrafico { get; set; } = Array.Empty<double>();
    public string[] EtiquetasGrafico { get; set; } = Array.Empty<string>();

    public List<Venta> UltimasVentas { get; set; } = new();
}