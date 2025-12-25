namespace MusicLibrary.Domain.Entities
{
    public class EmailConfirmationToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }

        public bool Used { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
