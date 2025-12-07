using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Repositories
{
    public interface IMediaRepository
    {
        Task Addsync(MediaItem item);
        Task<IEnumerable<MediaItem>> GetAllAsync();
    }
}
