using JournalToDoMix.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Services
{
    public class ActivitiesCategoriesServices : IActivitiesCategoriesServices
    {
        private readonly ApplicationDbContext _dbContext;
        public ActivitiesCategoriesServices(ApplicationDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public void AddCategory(ActivityCategory category)
        {
            _dbContext.ActivityCategories.Add(category);
        }

        public ActivityCategory? FindCategory(string categoryName)
        {
            return _dbContext.ActivityCategories.FirstOrDefault(x => x.CategoryName == categoryName);
        }

        public async Task<List<string>> GetCategoriesNamesAsync()
        {
            return await _dbContext.ActivityCategories
                                   .Select(c => c.CategoryName)
                                   .ToListAsync();
        }

        public ActivityCategory GetCategory(string categoryName)
        {
            var activityCategory = FindCategory(categoryName);
            if (activityCategory == null)
            {
                activityCategory = new ActivityCategory
                {
                    CategoryName = categoryName
                };
                AddCategory(activityCategory);
            }

            return activityCategory;
        }
    }
}
