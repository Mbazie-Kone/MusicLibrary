using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Auth.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task SaveChangeAsync(CancellationToken ct = default);
    }
}
