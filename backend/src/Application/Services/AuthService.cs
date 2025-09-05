using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _cfg;

    public AuthService(AppDbContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }

    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        // Email único
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new InvalidOperationException("El correo ya está registrado.");

        // Si solicita admin, debe proveer AdminCreationPassword válida (comparada contra hash en BD)
        var role = dto.DesiredRole?.ToLowerInvariant() == "admin" ? "admin" : "user";
        if (role == "admin")
        {
            var setting = await _db.SystemSettings.FirstOrDefaultAsync(s => s.Key == "AdminCreationPasswordHash");
            if (setting == null || !BCrypt.Net.BCrypt.Verify(dto.AdminCreationPassword ?? "", setting.Value))
                throw new UnauthorizedAccessException("Clave de creación de admin inválida.");
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User
        {
            Email = dto.Email,
            Name  = dto.Name,
            PasswordHash = hash,
            Role = role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null)
            return null;

        // Verificar lockout
        if (user.IsLockedManually)
            throw new UnauthorizedAccessException("Usuario bloqueado por el administrador.");

        if (user.LockoutEndUtc.HasValue && user.LockoutEndUtc.Value > DateTime.UtcNow)
            throw new UnauthorizedAccessException($"Usuario bloqueado hasta {user.LockoutEndUtc:O}.");

        // Validar password
        var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!ok)
        {
            await ApplyLockoutAsync(user);
            return null;
        }

        // Éxito: reset contadores
        user.FailedLoginCount = 0;
        user.LockoutEndUtc = null;
        user.LockoutLevel = 0;
        await _db.SaveChangesAsync();

        return GenerateJwt(user);
    }

    private async Task ApplyLockoutAsync(User user)
    {
        user.FailedLoginCount += 1;

        // Primera barrera: 3 intentos fallidos => 1 minuto
        if (user.FailedLoginCount >= 3 && user.LockoutLevel == 0)
        {
            user.LockoutEndUtc = DateTime.UtcNow.AddMinutes(1);
            user.LockoutLevel = 1;
            user.FailedLoginCount = 0; // reinicia el contador tras aplicar sanción
        }
        // Segunda barrera: siguiente fallo (ya pasó por 1min) => 5 minutos
        else if (user.LockoutLevel == 1 && user.FailedLoginCount >= 1)
        {
            user.LockoutEndUtc = DateTime.UtcNow.AddMinutes(5);
            user.LockoutLevel = 2;
            user.FailedLoginCount = 0;
        }
        // Tercera barrera: nuevo fallo tras el de 5min => bloqueo manual
        else if (user.LockoutLevel == 2 && user.FailedLoginCount >= 1)
        {
            user.IsLockedManually = true;
            user.LockoutLevel = 3;
            user.FailedLoginCount = 0;
        }

        await _db.SaveChangesAsync();
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("name", user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
