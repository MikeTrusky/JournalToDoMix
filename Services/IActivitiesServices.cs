using JournalToDoMix.Models;
using JournalToDoMix.ViewModels;

namespace JournalToDoMix.Services
{
    public interface IActivitiesServices
    {
        void UpdateActivitiesCompletedStatus(DateTime now, string? userId);        
        IQueryable<Activity> GetActivitiesBetweenDays(DateTime startOfWeek, int daysToShow, string? userId);
        List<Activity> GetPlannedActivities(DateTime now, int pageSize, int pageNumber, string? userId);
        List<Activity> GetCurrentActivities(DateTime now, int pageSize, int pageNumber, string? userId);
        List<Activity> GetPreviousActivities(DateTime now, int pageSize, int pageNumber, string? userId);
        List<StatisticsViewModel> GetEachActivityCount(string? userId);
        List<StatisticsViewModel> GetEachActivityTime(string? userId);
        int GetPlannedActivitiesCount(DateTime now, string? userId);
        int GetCurrentActivitiesCount(DateTime now, string? userId);
        int GetPreviousActivitiesCount(DateTime now, string? userId);  
        
        void AddActivity(Activity activity);
        void UpdateActivity(Activity activity);
        Activity? GetActivity(int? id);
        void RemoveActivity(Activity activity);
    }
}
