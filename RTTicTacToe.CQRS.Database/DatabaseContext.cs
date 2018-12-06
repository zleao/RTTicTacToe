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

        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>().HasIndex(b => b.Id);
        }
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
