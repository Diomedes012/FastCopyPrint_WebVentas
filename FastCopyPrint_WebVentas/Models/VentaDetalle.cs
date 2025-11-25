using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class VentaDetalle
{
    [Key]
    public int VentaDetalleId { get; set; }

    public int VentaId { get; set; }
    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    [ForeignKey(nameof(VentaId))]
    public virtual Venta Venta { get; set; }


    [ForeignKey(nameof(ProductoId))]
    public virtual Producto Producto { get; set; }
}
