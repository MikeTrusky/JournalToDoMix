using JournalToDoMix.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JournalToDoMix.Services
{
    public class ActivitiesServices : IActivitiesServices
    {
        private readonly ApplicationDbContext _dbContext;

        public ActivitiesServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<Activity> GetFilteredActivities(Expression<Func<Activity, bool>> filter)
        {
            return _dbContext.Activities
                    .Include(t => t.ActivityTitle)
                    .Include(c => c.ActivityCategory)
                    .Where(filter)
                    .AsNoTracking();
        }

        #region CurrentActivities
        public List<Activity> GetCurrentActivities(DateTime now, int pageSize, int pageNumber)
        {
            var skipNumber = (pageNumber - 1) * pageSize;
            return GetFilteredActivities(a => a.StartedAt <= now && !a.IsCompleted)
                    .AsEnumerable()
                    .Where(a => a.StartedAt.Add(a.DurationPlanned) >= now)
                    .Skip(skipNumber)
                    .Take(pageSize)
                    .ToList();
        }

        public int GetCurrentActivitiesCount(DateTime now)
        {
            return GetFilteredActivities(a => a.StartedAt <= now && !a.IsCompleted)
                    .AsEnumerable()
                    .Where(a => a.StartedAt.Add(a.DurationPlanned) >= now)
                    .Count();
        }
        #endregion

        #region PlannedActivities
        public List<Activity> GetPlannedActivities(DateTime now, int pageSize, int pageNumber)
        {
            var skipNumber = (pageNumber - 1) * pageSize;
            return GetFilteredActivities(a => a.StartedAt > now)
                    .Skip(skipNumber)
                    .Take(pageSize)
                    .ToList();            
        }

        public int GetPlannedActivitiesCount(DateTime now)
        {
            return GetFilteredActivities(a => a.StartedAt > now)
                    .Count();
        }
        #endregion

        #region PreviousActivities
        public List<Activity> GetPreviousActivities(DateTime now, int pageSize, int pageNumber)
        {
            var skipNumber = (pageNumber - 1) * pageSize;
            return GetFilteredActivities(a => a.IsCompleted)
                    .Skip(skipNumber)
                    .Take(pageSize)
                    .ToList();            
        }

        public int GetPreviousActivitiesCount(DateTime now)
        {
            return GetFilteredActivities(a => a.IsCompleted)
                    .Count();
        }
        #endregion

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
