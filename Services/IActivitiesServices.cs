using JournalToDoMix.Models;

namespace JournalToDoMix.Services
{
    public interface IActivitiesServices
    {
        void UpdateActivitiesCompletedStatus(DateTime now);
        List<Activity> GetPlannedActivities(DateTime now);
        List<Activity> GetCurrentActivities(DateTime now);
        List<Activity> GetPreviousActivities(DateTime now);
    }
}
