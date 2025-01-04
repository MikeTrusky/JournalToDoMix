using JournalToDoMix.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Services
{
    public class ActivitiesTitlesServices : IActivitiesTitlesServices
    {
        private readonly ApplicationDbContext _dbContext;

        public ActivitiesTitlesServices(ApplicationDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public void AddTitle(ActivityTitle title)
        {
            _dbContext.ActivityTitles.Add(title);
        }

        public ActivityTitle? FindTitle(string title)
        {
            return _dbContext.ActivityTitles.FirstOrDefault(x => x.Title == title);
        }

        public ActivityTitle GetTitle(string title, string? description)
        {
            var activityTitle = FindTitle(title);
            if (activityTitle == null)
            {
                activityTitle = new ActivityTitle
                {
                    Title = title,
                    Description = description
                };
                AddTitle(activityTitle);
            }
            return activityTitle;
        }

        public async Task<List<string>> GetTitlesStartsWithAsync(string query)
        {
            return await _dbContext.ActivityTitles
                                   .Where(t => t.Title.StartsWith(query))
                                   .Select(t => t.Title)
                                   .ToListAsync();
        }
    }
}
