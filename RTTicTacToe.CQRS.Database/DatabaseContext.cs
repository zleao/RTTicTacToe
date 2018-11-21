using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RTTicTacToe.CQRS.Database.Models;

namespace RTTicTacToe.CQRS.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Game { get; set; }
    }

    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite("Data Source=CQRSGame.db");

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
