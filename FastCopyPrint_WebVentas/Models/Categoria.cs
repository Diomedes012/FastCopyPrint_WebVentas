using System.ComponentModel.DataAnnotations;

namespace FastCopyPrint_Ventas.Models;

public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, MaxLength(300, ErrorMessage = "La descripcion no puede exceder los 300 caracteres!")]
    public string Descripcion { get; set; }

}
