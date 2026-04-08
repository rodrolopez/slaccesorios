using Microsoft.AspNetCore.Identity;
using YuyoDev.Domain.Entities;

namespace YuyoDev.WebAPI.Seeders;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        // Traemos la herramienta de .NET que maneja los roles
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //para manejar usuarios
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Definimos los roles que queremos en nuestro sistema
        string[] roleNames = { "SupAdmin", "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            // Verificamos si el rol ya existe para no duplicarlo
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Si no existe, lo creamos
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        // 2. Crear al Dios del Sistema (SupAdmin)
        var supAdminEmail = "yuyo@yuyodev.com";
        var supAdminUser = await userManager.FindByEmailAsync(supAdminEmail);

        if (supAdminUser == null)
        {
            var newSupAdmin = new ApplicationUser
            {
                UserName = supAdminEmail,
                Email = supAdminEmail,
                FirstName = "Rodrigo",
                LastName = "López"
            };

            var createPowerUser = await userManager.CreateAsync(newSupAdmin, "MasterYuyo2026!");
            if (createPowerUser.Succeeded)
            {
                await userManager.AddToRoleAsync(newSupAdmin, "SupAdmin");
            }
        }
        else
        {
            // ¡NUEVO! Si el usuario ya existe en la base de datos, le forzamos la corona.
            if (!await userManager.IsInRoleAsync(supAdminUser, "SupAdmin"))
            {
                await userManager.AddToRoleAsync(supAdminUser, "SupAdmin");
            }
        }
    }
}