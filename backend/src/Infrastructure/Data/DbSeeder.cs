using BCrypt.Net;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        // 1) Guardar hash de la "clave de creación de admin" en Settings si no existe
        const string settingKey = "AdminCreationPasswordHash";
        if (!await db.SystemSettings.AnyAsync(s => s.Key == settingKey))
        {
            // Clave predeterminada que debes cambiar en producción
            var adminCreationPassword = "Admin@2025!";
            var hash = BCrypt.Net.BCrypt.HashPassword(adminCreationPassword);
            db.SystemSettings.Add(new SystemSetting { Key = settingKey, Value = hash });
            await db.SaveChangesAsync();
        }

        // 2) Semilla de usuarios (admin y user) si no existen
        if (!await db.Users.AnyAsync())
        {
            var adminPassHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            var userPassHash  = BCrypt.Net.BCrypt.HashPassword("User123!");

            db.Users.Add(new User
            {
                Email = "admin@demo.com",
                Name  = "Admin Demo",
                Role  = "admin",
                PasswordHash = adminPassHash
            });

            db.Users.Add(new User
            {
                Email = "user@demo.com",
                Name  = "Usuario Demo",
                Role  = "user",
                PasswordHash = userPassHash
            });

            await db.SaveChangesAsync();
        }
    }
}
