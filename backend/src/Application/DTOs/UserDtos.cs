namespace Application.DTOs;

public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Name  { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
}

public class UpdateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Name  { get; set; } = string.Empty;
    public string Role  { get; set; } = "user";
    public string? Password { get; set; } // opcional para cambio
}
