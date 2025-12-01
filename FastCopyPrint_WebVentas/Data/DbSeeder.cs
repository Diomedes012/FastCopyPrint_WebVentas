using Microsoft.AspNetCore.Identity;

namespace FastCopyPrint_WebVentas.Data;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider service)
    {
        var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

        var adminEmail = "admin@fastcopy.com";
        var defaultPassword = "Admin$1234";

        // Buscamos si ya existe
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            // CREAR USUARIO NUEVO ---
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Nombre = "Encargado Principal",
                Activo = true,

                FechaRegistro = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, defaultPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Encargado");
            }
        }
        else
        {
            //REPARAR USUARIO EXISTENTE ---

            if (!await userManager.CheckPasswordAsync(adminUser, defaultPassword))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, defaultPassword);
            }

            //Asegurar que tenga el rol de Encargado
            if (!await userManager.IsInRoleAsync(adminUser, "Encargado"))
            {
                await userManager.AddToRoleAsync(adminUser, "Encargado");
            }

            // Asegurar que esté Activo
            if (!adminUser.Activo)
            {
                adminUser.Activo = true;
                await userManager.UpdateAsync(adminUser);
            }
        }
    }
}