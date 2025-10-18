using DSD.Core.Entities;

namespace DSD.API.Services
{
    public interface ITokenService
    {
        string BuildToken(User user);
    }
}
