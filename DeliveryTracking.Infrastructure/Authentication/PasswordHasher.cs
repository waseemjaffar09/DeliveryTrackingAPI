using BC = BCrypt.Net.BCrypt;
using DeliveryTracking.Application.Interfaces.Authentication;

namespace DeliveryTracking.Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BC.Verify(password, passwordHash);
        }
    }
}
