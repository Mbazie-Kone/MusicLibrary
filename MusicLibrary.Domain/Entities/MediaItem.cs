using MusicLibrary.Domain.Enums;

namespace MusicLibrary.Domain.Entities
{
    public class MediaItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public MediaStatus Status { get; set; } = MediaStatus.Pending;
        public string? Duration { get; set; }
        public string? Bitrate { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
    }
}
