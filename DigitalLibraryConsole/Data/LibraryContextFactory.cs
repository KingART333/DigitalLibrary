using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DigitalLibraryConsole.Data
{
    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
    {
        public LibraryContext CreateDbContext(string[] args)
        {
            // 🔒 Hardcoded connection string (only for EF CLI use)
            var connectionString = "Data Source=C:\\Repos\\DigitalLibrary\\DigitalLibraryConsole\\bin\\Debug\\net8.0\\library.db";

            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new LibraryContext(optionsBuilder.Options);
        }
    }
}
