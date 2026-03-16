using BCrypt.Net;

namespace Novus.Infrastructure.Authentication;

public static class PasswordHasher
{
    public static string Hash(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    public static bool Verify(string password, string hash) => BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}
