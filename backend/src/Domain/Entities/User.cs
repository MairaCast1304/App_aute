namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string Role { get; set; } = "user"; // "admin" | "user"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Seguridad: lockouts escalonados
    public int FailedLoginCount { get; set; } = 0;
    public DateTime? LockoutEndUtc { get; set; } = null;
    /// <summary>
    /// 0 = sin bloqueo; 1 = ya recibió 1min; 2 = ya recibió 5min; 3 = bloqueo manual requerido
    /// </summary>
    public int LockoutLevel { get; set; } = 0;
    public bool IsLockedManually { get; set; } = false;
}