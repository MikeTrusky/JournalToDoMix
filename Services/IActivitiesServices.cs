using JournalToDoMix.Models;
using JournalToDoMix.ViewModels;
using System.Linq.Expressions;

namespace JournalToDoMix.Services
{
    public interface IActivitiesServices
    {
        void UpdateActivitiesCompletedStatus(DateTime now);
        IQueryable<Activity> GetFilteredActivities(Expression<Func<Activity, bool>> filter);
        List<Activity> GetPlannedActivities(DateTime now, int pageSize, int pageNumber);
        List<Activity> GetCurrentActivities(DateTime now, int pageSize, int pageNumber);
        List<Activity> GetPreviousActivities(DateTime now, int pageSize, int pageNumber);
        List<StatisticsViewModel> GetEachActivityCount();
        List<StatisticsViewModel> GetEachActivityTime();
        int GetPlannedActivitiesCount(DateTime now);
        int GetCurrentActivitiesCount(DateTime now);
        int GetPreviousActivitiesCount(DateTime now);  
        
        void AddActivity(Activity activity);
        void UpdateActivity(Activity activity);
        Activity? GetActivity(int? id);
        void RemoveActivity(Activity activity);

    }
}
