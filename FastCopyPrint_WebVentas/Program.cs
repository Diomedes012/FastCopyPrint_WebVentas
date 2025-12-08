using FastCopyPrint_WebVentas.Components;
using FastCopyPrint_WebVentas.Components.Account;
using FastCopyPrint_WebVentas.Data;
using FastCopyPrint_WebVentas.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace FastCopyPrint_WebVentas
{
    public static class Program
    {
        // CAMBIO 1: Cambiar 'void' a 'async Task' para permitir el await del seeder
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            builder.Services.AddScoped<CategoriasService>();
            builder.Services.AddScoped<ProductosService>();
            builder.Services.AddScoped<VentasService>();
            builder.Services.AddScoped<DashboardService>();
            builder.Services.AddScoped<CatalogoService>();
            builder.Services.AddScoped<CarritoService>();
            builder.Services.AddScoped<VentaService>();
            builder.Services.AddMudServices();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString),
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Singleton);

            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
                // CAMBIO 2: ¡Importante! Agregar Roles para que funcione RoleManager en el Seeder
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapAdditionalIdentityEndpoints();

            // CAMBIO 3: Ejecutar el Seeder
            // Creamos un scope temporal para obtener los servicios necesarios
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await FastCopyPrint_WebVentas.Data.DbSeeder.SeedAsync(services);
            }

            // Ejecutar la aplicación
            await app.RunAsync();
        }
    }
}