using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class ListaDeseo
{
    [Key]
    public int ListaDeseoId { get; set; }

    public int ClienteId { get; set; }

    public int ProductoId { get; set; }

    public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ClienteId))]
    public virtual Cliente cliente { get; set; }


    [ForeignKey(nameof(ProductoId))]
    public virtual Producto Producto { get; set; }

}
