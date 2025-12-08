using FastCopyPrint_Ventas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastCopyPrint_WebVentas.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider service)
    {
        await SeedEncargadoAsync(service);
        await SeedClientesYVentasAsync(service);
    }

    private static async Task SeedEncargadoAsync(IServiceProvider service)
    {
        var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
        var adminEmail = "admin@fastcopy.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Nombre = "Encargado Principal",
                Activo = true,
                FechaRegistro = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(adminUser, "Admin$1234");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Encargado");
            }
        }
    }

    private static async Task SeedClientesYVentasAsync(IServiceProvider service)
    {
        var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
        var factory = service.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        await using var context = await factory.CreateDbContextAsync();


        // Cliente 1: Juan
        var emailJuan = "juan@gmail.com";
        var userJuan = await userManager.FindByEmailAsync(emailJuan);
        Cliente? clienteJuan = null;

        if (userJuan == null)
        {
            userJuan = new ApplicationUser
            {
                UserName = emailJuan,
                Email = emailJuan,
                EmailConfirmed = true,
                Nombre = "Juan Pérez",
                Activo = true,
                FechaRegistro = DateTime.UtcNow.AddMonths(-1)
            };
            await userManager.CreateAsync(userJuan, "Cliente123!");
            await userManager.AddToRoleAsync(userJuan, "Cliente");

            clienteJuan = new Cliente { UsuarioId = userJuan.Id, DireccionEnvio = "Av. Libertad #45" };
            context.Clientes.Add(clienteJuan);
            await context.SaveChangesAsync();
        }
        else
        {
            clienteJuan = await context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userJuan.Id);
        }

        // Cliente 2: María
        var emailMaria = "maria@gmail.com";
        var userMaria = await userManager.FindByEmailAsync(emailMaria);
        Cliente? clienteMaria = null;

        if (userMaria == null)
        {
            userMaria = new ApplicationUser
            {
                UserName = emailMaria,
                Email = emailMaria,
                EmailConfirmed = true,
                Nombre = "María López",
                Activo = true,
                FechaRegistro = DateTime.UtcNow.AddMonths(-2)
            };
            await userManager.CreateAsync(userMaria, "Cliente123!");
            await userManager.AddToRoleAsync(userMaria, "Cliente");

            clienteMaria = new Cliente { UsuarioId = userMaria.Id, DireccionEnvio = "Calle Salcedo #10" };
            context.Clientes.Add(clienteMaria);
            await context.SaveChangesAsync();
        }
        else
        {
            clienteMaria = await context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userMaria.Id);
        }

        if (!await context.Ventas.AnyAsync() && clienteJuan != null && clienteMaria != null)
        {
            var ventas = new List<Venta>
            {
                new Venta
                {
                    ClienteId = clienteJuan.ClienteId,
                    VendedorId = null, // Fue compra web, sin vendedor
                    MetodoPagoId = 1,  // Efectivo
                    FechaVenta = DateTime.UtcNow.AddDays(-1),
                    TotalVenta = 800.00m,
                    EstadoVenta = "Completada",
                    NumeroFactura = "FAC-0001",
                    Detalles = new List<VentaDetalle>
                    {
                        new VentaDetalle { ProductoId = 1, Cantidad = 1, PrecioUnitario = 450.00m },
                        new VentaDetalle { ProductoId = 2, Cantidad = 1, PrecioUnitario = 350.00m }
                    }
                },

                // Venta 2: María (Hoy)
                new Venta
                {
                    ClienteId = clienteMaria.ClienteId,
                    VendedorId = null,
                    MetodoPagoId = 2, // Tarjeta
                    FechaVenta = DateTime.UtcNow.AddHours(-2),
                    TotalVenta = 3150.00m,
                    EstadoVenta = "Completada",
                    NumeroFactura = "FAC-0002",
                    Detalles = new List<VentaDetalle>
                    {
                        new VentaDetalle { ProductoId = 4, Cantidad = 2, PrecioUnitario = 450.00m },
                        new VentaDetalle { ProductoId = 1, Cantidad = 5, PrecioUnitario = 450.00m }
                    }
                },

                // Venta 3: Juan (Hace 5 días)
                new Venta
                {
                    ClienteId = clienteJuan.ClienteId,
                    VendedorId = null,
                    MetodoPagoId = 1,
                    FechaVenta = DateTime.UtcNow.AddDays(-5),
                    TotalVenta = 400.00m,
                    EstadoVenta = "Completada",
                    NumeroFactura = "FAC-0003",
                    Detalles = new List<VentaDetalle>
                    {
                        new VentaDetalle { ProductoId = 3, Cantidad = 5, PrecioUnitario = 80.00m }
                    }
                }
            };

            context.Ventas.AddRange(ventas);
            await context.SaveChangesAsync();
        }
    }
}