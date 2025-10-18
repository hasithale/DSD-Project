using BCrypt.Net;

namespace DSD.API.Services
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 12; // adjustable; 12 is safe for dev/prod
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        public bool Verify(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
