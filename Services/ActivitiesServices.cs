using JournalToDoMix.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Services
{
    public class ActivitiesServices : IActivitiesServices
    {
        private readonly ApplicationDbContext _dbContext;

        public ActivitiesServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Activity> GetCurrentActivities(DateTime now)
        {
            return  _dbContext.Activities
                    .Where(a => a.StartedAt <= now && !a.IsCompleted)
                    .AsNoTracking()
                    .AsEnumerable()
                    .Where(a => a.StartedAt.Add(a.DurationPlanned) >= now)
                    .ToList();
        }

        public List<Activity> GetPlannedActivities(DateTime now)
        {
            return  _dbContext.Activities
                    .Where(a => a.StartedAt > now)
                    .AsNoTracking()
                    .ToList();
        }

        public List<Activity> GetPreviousActivities(DateTime now)
        {
            return  _dbContext.Activities
                    .Where(a => a.IsCompleted)
                    .AsNoTracking()
                    .ToList();
        }

        public void UpdateActivitiesCompletedStatus(DateTime now)
        {
            var incompletedActivities = _dbContext.Activities
                            .Where(a => !a.IsCompleted)
                            .ToList();

            bool hasChange = false;

            foreach (var activity in incompletedActivities)
            {
                var endTime = activity.StartedAt.Add(activity.DurationPlanned);
                if (endTime < now)
                {
                    activity.IsCompleted = true;
                    _dbContext.Activities.Update(activity);
                    hasChange = true;
                }
            }

            if (hasChange)
                _dbContext.SaveChanges();
        }
    }
}
