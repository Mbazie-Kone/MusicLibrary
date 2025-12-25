namespace MusicLibrary.Api.Dtos
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
