using Microsoft.EntityFrameworkCore;
using MusicLibrary.Application.Auth.Interfaces;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Infrastructure.DbContexts;

namespace MusicLibrary.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicLibraryDbContext _db;

        public UserRepository(MusicLibraryDbContext db)
        {
            _db = db;
        }

        // Alternative approach without using return
        public Task AddAsync(User user, CancellationToken ct = default) => _db.Users.AddAsync(user, ct).AsTask();

        // Alternative approach without using return
        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) => _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        // Standard method implementation
        public Task SaveChangeAsync(CancellationToken ct = default)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}
