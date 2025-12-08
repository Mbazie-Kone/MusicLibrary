using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Repositories
{
    public interface IMediaRepository
    {
        Task Addsync(MediaItem item);
        Task DeleteAsync(int id);
        Task<List<MediaItem>> GetAllAsync();
        Task<MediaItem?> GetByIdAsync(int id);
        Task UpdateAsync(MediaItem item);

    }
}
