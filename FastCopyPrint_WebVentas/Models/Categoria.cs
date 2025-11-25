using System.ComponentModel.DataAnnotations;

namespace FastCopyPrint_Ventas.Models;

public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, MaxLength(300)]
    public string Descripcion { get; set; }

}
