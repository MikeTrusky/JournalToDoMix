using JournalToDoMix.Models;
using JournalToDoMix.ViewModels;

namespace JournalToDoMix.Services
{
    public interface IActivitiesServices
    {
        void UpdateActivitiesCompletedStatus(DateTime now);
        List<Activity> GetPlannedActivities(DateTime now, int pageSize, int pageNumber);
        List<Activity> GetCurrentActivities(DateTime now, int pageSize, int pageNumber);
        List<Activity> GetPreviousActivities(DateTime now, int pageSize, int pageNumber);
        List<StatisticsViewModel> GetEachActivityCount();
        List<StatisticsViewModel> GetEachActivityTime();
        int GetPlannedActivitiesCount(DateTime now);
        int GetCurrentActivitiesCount(DateTime now);
        int GetPreviousActivitiesCount(DateTime now);
    }
}
