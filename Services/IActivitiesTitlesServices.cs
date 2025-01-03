using JournalToDoMix.Models;

namespace JournalToDoMix.Services
{
    public interface IActivitiesTitlesServices
    {
        ActivityTitle? FindTitle(string title);
        void AddTitle(ActivityTitle title);
        Task<List<string>> GetTitlesStartsWithAsync(string query);
    }
}
