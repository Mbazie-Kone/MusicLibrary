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
            return await _context.MediaItems
                .OrderByDescending(m => m.UploadedAt)
                .ToListAsync();
        }

        public async Task<MediaItem?> GetByIdAsync(int id)
        {
            return await _context.MediaItems.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateAsync(MediaItem item)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MediaItem item)
        {
            _context.MediaItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        // TO DO
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();

        }
    }
}
