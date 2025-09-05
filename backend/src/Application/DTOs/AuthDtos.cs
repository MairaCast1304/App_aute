namespace Application.DTOs;

public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Name  { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Rol deseado ("user" por defecto). Si "admin", requiere AdminCreationPassword.
    public string DesiredRole { get; set; } = "user";
    public string? AdminCreationPassword { get; set; }
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
