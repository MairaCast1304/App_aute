using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db) => _db = db;

    public Task<List<User>> GetAllAsync() => _db.Users.OrderBy(u => u.Email).ToListAsync();

    public Task<User?> GetAsync(Guid id) => _db.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User> CreateAsync(CreateUserDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new InvalidOperationException("El correo ya está registrado.");

        var user = new User
        {
            Email = dto.Email,
            Name  = dto.Name,
            Role  = (dto.Role?.ToLowerInvariant() == "admin" ? "admin" : "user"),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await GetAsync(id) ?? throw new KeyNotFoundException("Usuario no encontrado.");

        // En edición de admin, validar que el email no duplique
        if (user.Email != dto.Email && await _db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new InvalidOperationException("El correo ya está registrado.");

        user.Email = dto.Email;
        user.Name  = dto.Name;
        user.Role  = (dto.Role?.ToLowerInvariant() == "admin" ? "admin" : "user");
        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetAsync(id) ?? throw new KeyNotFoundException("Usuario no encontrado.");
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateProfileAsync(Guid id, UpdateProfileDto dto)
    {
        var user = await GetAsync(id) ?? throw new KeyNotFoundException("Usuario no encontrado.");
        user.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        await _db.SaveChangesAsync();
    }

    public async Task UnlockAsync(Guid id)
    {
        var user = await GetAsync(id) ?? throw new KeyNotFoundException("Usuario no encontrado.");
        user.IsLockedManually = false;
        user.LockoutEndUtc = null;
        user.LockoutLevel = 0;
        user.FailedLoginCount = 0;
        await _db.SaveChangesAsync();
    }
}
