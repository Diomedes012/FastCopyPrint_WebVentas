using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FastCopyPrint_WebVentas.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool Activo { get; set; }
}
