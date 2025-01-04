using JournalToDoMix.Models;

namespace JournalToDoMix.Services
{
    public interface IActivitiesTitlesServices
    {
        ActivityTitle GetTitle(string title, string? description);
        ActivityTitle? FindTitle(string title);
        void AddTitle(ActivityTitle title);
        Task<List<string>> GetTitlesStartsWithAsync(string query);
    }
}
