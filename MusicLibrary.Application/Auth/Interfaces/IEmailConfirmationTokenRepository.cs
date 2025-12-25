using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Auth.Interfaces
{
    public interface IEmailConfirmationTokenRepository
    {
        Task AddAsync(EmailConfirmationToken token, CancellationToken ct = default);
        Task<EmailConfirmationToken?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task SaveChangeAsync(CancellationToken ct = default);
    }
}
