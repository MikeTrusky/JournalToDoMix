using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Action> Actions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>().HasData(
                new Action { Id = 1, Title = "Reading", Description = "Reading a book", IsCompleted = false, DurationPlanned = new TimeSpan(0, 30, 0), StartedAt = new DateTime(2024, 10, 18, 15, 0, 0, DateTimeKind.Local)}
                );
        }
    }
}
