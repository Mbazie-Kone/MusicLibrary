using MusicLibrary.Application.Auth.Commands;

namespace MusicLibrary.Application.Auth.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterUserCommand command, CancellationToken ct = default);
    }
}
