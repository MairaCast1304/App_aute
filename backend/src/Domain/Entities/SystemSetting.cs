namespace Domain.Entities;

public class SystemSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;    // e.g. "AdminCreationPasswordHash"
    public string Value { get; set; } = string.Empty;  // hash BCrypt
}
