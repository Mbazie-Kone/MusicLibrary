namespace MusicLibrary.Application.Auth.Commands
{
    public record RegisterUserCommand(
        string Email,
        string Password
    );
}
