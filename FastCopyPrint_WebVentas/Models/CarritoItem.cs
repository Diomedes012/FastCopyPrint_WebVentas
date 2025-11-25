using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class CarritoItem
{
    [Key]
    public int CarritoItemId { get; set; }

    public int CarritoId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;


    [ForeignKey(nameof(CarritoId))]
    public virtual Carrito Carrito { get; set; }


    [ForeignKey(nameof(ProductoId))]
    public virtual Producto Producto { get; set; }
}
