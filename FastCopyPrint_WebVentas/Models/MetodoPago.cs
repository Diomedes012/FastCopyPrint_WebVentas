using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class MetodoPago
{
    [Key]
    public int MetodoPagoId { get; set; }

    [MaxLength(50)]
    public string Nombre { get; set; }

    [MaxLength(250)]
    public string Descripcion { get; set; }
}
