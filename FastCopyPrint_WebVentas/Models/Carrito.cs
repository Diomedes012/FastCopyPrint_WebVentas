using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class Carrito
{
    [Key]
    public int CarritoId { get; set; }

    public int ClienteId { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ClienteId))]
    public virtual Cliente Cliente { get; set; }

    public virtual ICollection<CarritoItem> Items { get; set; }
}
