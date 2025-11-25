using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class Producto
{
    [Key]
    public int ProductoId { get; set; }

    [Required, MaxLength(200)]
    public string Nombre { get; set; }

    [Required, MaxLength(1000)]
    public string Descripcion { get; set; }

    [Required]
    public decimal Precio { get; set; }

    [Required]
    public int Stock { get; set; }

    public int? CategoriaId { get; set; }

    [Required]
    public bool EstaActivo { get; set; }

    [MaxLength(300)]
    public string ImagenUrl { get; set; }

    [ForeignKey(nameof(CategoriaId))]
    public virtual Categoria Categoria { get; set; }

}
