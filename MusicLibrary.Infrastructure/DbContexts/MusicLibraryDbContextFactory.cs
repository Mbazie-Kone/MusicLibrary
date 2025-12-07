using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MusicLibrary.Infrastructure.DbContexts
{
    public class MusicLibraryDbContextFactory : IDesignTimeDbContextFactory<MusicLibraryDbContext>
    {
        public MusicLibraryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicLibraryDbContext>();

            // Connection string USED ONLY for dotnet ef commands
            var connectionString =
                "Server=localhost,1433;Database=MusicLibraryDb;User Id=sa;Password=12345678aA!;TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(connectionString);

            return new MusicLibraryDbContext(optionsBuilder.Options);
        }

    }
}
