using FastCopyPrint_Ventas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastCopyPrint_WebVentas.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
    {
        var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        var contexto = service.GetService<ApplicationDbContext>();

        await CheckCreateRole(roleManager, "Admin");
 
        var adminEmail = "admin@fastcopy.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true, 
                Nombre = "Administrador Principal",
                Activo = true,
                FechaRegistro = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(adminUser, "Admin$1234");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        await SeedMetodosPagoAsync(service);
    }
    public static async Task SeedMetodosPagoAsync(IServiceProvider service)
    {
       
        var context = service.GetRequiredService<ApplicationDbContext>();

        if (!await context.MetodosPago.AnyAsync())
        {
            context.MetodosPago.AddRange(
                new MetodoPago
                {
                    Nombre = "Tarjeta",
                    Descripcion = "Pago procesado vía tarjeta de Crédito/Débito"
                },
                new MetodoPago
                {
                    Nombre = "Efectivo",
                    Descripcion = "Pago contra entrega en el local o domicilio"
                }
            );

            await context.SaveChangesAsync();
        }
    }

    private static async Task CheckCreateRole(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
