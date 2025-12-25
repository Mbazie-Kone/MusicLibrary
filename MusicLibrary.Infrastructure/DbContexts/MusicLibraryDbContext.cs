using Microsoft.EntityFrameworkCore;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.DbContexts
{
    public class MusicLibraryDbContext : DbContext
    {
        public MusicLibraryDbContext(DbContextOptions<MusicLibraryDbContext> options) : base(options)
        {
        }

        // “This property represents the MediaItems table.”
        public DbSet<MediaItem> MediaItems { get; set; }

        // Override for additional configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional Fluent API configurations
            modelBuilder.Entity<MediaItem>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.FileType)
                    .HasMaxLength(50);
                entity.Property(e => e.FileSize);
                entity.Property(e => e.UploadedAt)
                    .HasDefaultValueSql("GETDATE()"); // Default value
            });
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<EmailConfirmationToken> EmailConfirmationTokens => Set<EmailConfirmationToken>();


    }
}
