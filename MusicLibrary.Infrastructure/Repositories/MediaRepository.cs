using MusicLibrary.Domain.Entities;
using MusicLibrary.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MusicLibrary.Infrastructure.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly MusicLibraryDbContext _context;

        public MediaRepository(MusicLibraryDbContext context)
        {
            _context = context;
        }

        public async Task Addsync(MediaItem item)
        {
            _context.MediaItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MediaItem>> GetAllAsync()
        {
            return await _context.MediaItems.ToListAsync();
        }
    }
}
