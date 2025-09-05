using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            var (hash, salt) = PasswordHelper.HashPassword("Admin123!");

            context.Users.Add(new User {
                Id = Guid.NewGuid(),
                Name = "Administrador",
                Email = "admin@demo.com",
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "admin",
                FailedLoginCount = 0,
                LockoutEndUtc = null
            });

            var (userHash, userSalt) = PasswordHelper.HashPassword("User123!");
            context.Users.Add(new User {
                Id = Guid.NewGuid(),
                Name = "Usuario",
                Email = "user@demo.com",
                PasswordHash = userHash,
                PasswordSalt = userSalt,
                Role = "user",
                FailedLoginCount = 0,
                LockoutEndUtc = null
            });

            context.SaveChanges();
        }
    }
}
