using JournalToDoMix.Models;

namespace JournalToDoMix.Services
{
    public interface IActivitiesCategoriesServices
    {
        ActivityCategory GetCategory(string categoryName);
        ActivityCategory? FindCategory(string categoryName);
        void AddCategory(ActivityCategory category);
        Task<List<string>> GetCategoriesNamesAsync();
    }
}
