using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        DbSet<Action> Actions { get; set; }
    }
}
