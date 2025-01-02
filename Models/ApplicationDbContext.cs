using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityTitle> ActivityTitles { get; set; }
        public DbSet<ActivityCategory> ActivityCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityTitle>(entity =>
            {
                entity.HasIndex(e => e.Title).IsUnique();
            });

            builder.Entity<ActivityCategory>(entity =>
            {
                entity.HasIndex(e => e.CategoryName).IsUnique();
            });

            builder.Entity<Activity>(entity =>
            {
                entity.HasOne(a => a.ActivityTitle)
                      .WithMany(t => t.Activities)
                      .HasForeignKey(a => a.ActivityTitleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.ActivityCategory)
                      .WithMany(c => c.Activities)
                      .HasForeignKey(a => a.ActivityCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);                
            });

            builder.Entity<ActivityCategory>().HasData(
                new ActivityCategory { Id = 1, CategoryName = "Reading" },
                new ActivityCategory { Id = 2, CategoryName = "Learning" },
                new ActivityCategory { Id = 3, CategoryName = "Gaming" },
                new ActivityCategory { Id = 4, CategoryName = "Chilling" },
                new ActivityCategory { Id = 5, CategoryName = "Physical working"}
            );

            builder.Entity<ActivityTitle>().HasData(
                new ActivityTitle { Id = 1, Title = "Reading a book", Description = "Spending some time on reading a book" },
                new ActivityTitle { Id = 2, Title = "Learning to draw", Description = "Testing some new drawing methods" },
                new ActivityTitle { Id = 3, Title = "Playing a game", Description = "Playing a new pc game" },
                new ActivityTitle { Id = 4, Title = "Programming an app", Description = "Programming new big project" },
                new ActivityTitle { Id = 5, Title = "Chilling in a rain", Description = "Watching an outside rain" },
                new ActivityTitle { Id = 6, Title = "Chilling in a room", Description = "Walking around the room" },
                new ActivityTitle { Id = 7, Title = "Painting the wall", Description = "Painting a garage wall"}
            );

            builder.Entity<Activity>().HasData(
                new Activity { Id = 11, ActivityTitleId = 1, ActivityCategoryId = 1, Description = "", IsCompleted = true, StartedAt = new DateTime(2024, 10, 18, 15, 0, 0, DateTimeKind.Local), DurationPlanned = new TimeSpan(0, 30, 0)},
                new Activity { Id = 12, ActivityTitleId = 3, ActivityCategoryId = 3, Description = "", IsCompleted = true, StartedAt = new DateTime(2024, 10, 23, 10, 50, 0, DateTimeKind.Local), DurationPlanned = new TimeSpan(0, 30, 0) },
                new Activity { Id = 13, ActivityTitleId = 4, ActivityCategoryId = 2, Description = "", IsCompleted = true, StartedAt = new DateTime(2024, 10, 23, 10, 0, 0, DateTimeKind.Local), DurationPlanned = new TimeSpan(1, 0, 0) },
                new Activity { Id = 14, ActivityTitleId = 6, ActivityCategoryId = 4, Description = "", IsCompleted = true, StartedAt = new DateTime(2024, 12, 3, 13, 30, 0, DateTimeKind.Local), DurationPlanned = new TimeSpan(0, 30, 0) },
                new Activity { Id = 15, ActivityTitleId = 5, ActivityCategoryId = 4, Description = "", IsCompleted = true, StartedAt = new DateTime(2024, 12, 3, 13, 0, 0, DateTimeKind.Local), DurationPlanned = new TimeSpan(1, 0, 0) },
                new Activity { Id = 16, ActivityTitleId = 5, ActivityCategoryId = 4, Description = "", IsCompleted = true, StartedAt = new DateTime(2024, 12, 3, 13, 0, 0, DateTimeKind.Local), DurationPlanned = new TimeSpan(1, 0, 0) },
                new Activity { Id = 17, ActivityTitleId = 7, ActivityCategoryId = 5, Description = "", IsCompleted = true, StartedAt = new DateTime(2024, 11, 5, 10, 0, 0, DateTimeKind.Local), DurationPlanned = new TimeSpan(5, 0, 0) }
            );


            //hardcoded generated id values, due to a problem with updating the database in ef 9.0 version
            builder.Entity<IdentityRole>().HasData(                
                new IdentityRole { Id = "3883a00e-35ba-4828-90df-7f2b5a79bafd", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "7320f988-1bb6-4d47-ba3f-c68fa46add99", Name = "User", NormalizedName = "USER" }
                );
        }
    }
}
