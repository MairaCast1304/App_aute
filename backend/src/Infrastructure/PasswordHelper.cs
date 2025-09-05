using System.Security.Cryptography;
using System.Text;

namespace Infrastructure;

public static class PasswordHelper
{
    public static (string hash, string salt) HashPassword(string password)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                var hash = Convert.ToBase64String(pbkdf2.GetBytes(20));
                return (hash, salt);
            }
        }
    }
    
    public static bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
        {
            var testHash = Convert.ToBase64String(pbkdf2.GetBytes(20));
            return hash == testHash;
        }
    }
}