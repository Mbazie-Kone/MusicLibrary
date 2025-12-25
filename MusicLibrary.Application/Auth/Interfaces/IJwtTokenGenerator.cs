using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Auth.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
