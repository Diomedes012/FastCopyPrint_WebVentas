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

        await CheckCreateRole(roleManager, "Admin");

        var adminEmail = "admin@fastcopy.com";
        var defaultPassword = "Admin$1234"; // Guardamos el pass en variable

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            // CASO 1: El usuario NO existe, lo creamos
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Nombre = "Administrador Principal",
                Activo = true,
                FechaRegistro = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, defaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // CASO 2: El usuario SI existe, verificamos si la contraseña es válida
            // Esto arregla el problema si la contraseña estaba corrupta o era vieja
            if (!await userManager.CheckPasswordAsync(adminUser, defaultPassword))
            {
                // Generamos un token para resetear la contraseña
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                // Reseteamos la contraseña a la correcta
                await userManager.ResetPasswordAsync(adminUser, token, defaultPassword);
            }

            // Aseguramos que tenga el rol
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // OPCIONAL: Asegurar que Activo = true si ya existía pero estaba desactivado
            if (!adminUser.Activo)
            {
                adminUser.Activo = true;
                await userManager.UpdateAsync(adminUser);
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
