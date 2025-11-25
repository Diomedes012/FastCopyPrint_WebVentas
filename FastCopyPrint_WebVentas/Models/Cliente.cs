using FastCopyPrint_WebVentas.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastCopyPrint_Ventas.Models;

public class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    [Required]
    public string UsuarioId { get; set; }

    [Required, MaxLength(200)]
    public string DireccionEnvio { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public virtual ApplicationUser Usuario { get; set; }

}
