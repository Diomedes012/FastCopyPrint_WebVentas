using FastCopyPrint_WebVentas.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class Venta
{
    [Key]
    public int VentaId { get; set; }

    public int ClienteId { get; set; }

    [MaxLength(450)]
    public string? VendedorId { get; set; }

    public int? MetodoPagoId { get; set; }

    public DateTime FechaVenta { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalVenta { get; set; }

    [MaxLength(50)]
    public string EstadoVenta { get; set; }

    [MaxLength(100)]
    public string NumeroFactura { get; set; }

    [ForeignKey(nameof(ClienteId))]
    public virtual Cliente? Cliente { get; set; }


    [ForeignKey(nameof(VendedorId))]
    public virtual ApplicationUser? Vendedor { get; set; }


    [ForeignKey(nameof(MetodoPagoId))]
    public virtual MetodoPago? MetodoPago { get; set; }


    public virtual ICollection<VentaDetalle> Detalles { get; set; }
}
