
namespace MusicLibrary.Application.Auth.Commands
{
    public record LoginCommand(
        string Email,
        string Password
     );
}
