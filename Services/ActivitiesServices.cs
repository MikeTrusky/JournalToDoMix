﻿using JournalToDoMix.Models;
using JournalToDoMix.ViewModels;
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
        public List<Activity> GetCurrentActivities(DateTime now, int pageSize, int pageNumber, string? userId)
        {
            var skipNumber = (pageNumber - 1) * pageSize;
            return GetFilteredActivities(a => a.StartedAt <= now && !a.IsCompleted && a.AppUserId == userId)
                    .AsEnumerable()
                    .Where(a => a.StartedAt.Add(a.DurationPlanned) >= now)
                    .Skip(skipNumber)
                    .Take(pageSize)
                    .ToList();
        }

        public int GetCurrentActivitiesCount(DateTime now, string? userId)
        {
            return GetFilteredActivities(a => a.StartedAt <= now && !a.IsCompleted && a.AppUserId == userId)
                    .AsEnumerable()
                    .Where(a => a.StartedAt.Add(a.DurationPlanned) >= now)
                    .Count();
        }
        #endregion

        #region PlannedActivities
        public List<Activity> GetPlannedActivities(DateTime now, int pageSize, int pageNumber, string? userId)
        {
            var skipNumber = (pageNumber - 1) * pageSize;
            return GetFilteredActivities(a => a.StartedAt > now && a.AppUserId == userId)
                    .Skip(skipNumber)
                    .Take(pageSize)
                    .ToList();            
        }

        public int GetPlannedActivitiesCount(DateTime now, string? userId)
        {
            return GetFilteredActivities(a => a.StartedAt > now && a.AppUserId == userId)
                    .Count();
        }
        #endregion

        #region PreviousActivities
        public List<Activity> GetPreviousActivities(DateTime now, int pageSize, int pageNumber, string? userId)
        {
            var skipNumber = (pageNumber - 1) * pageSize;
            return GetFilteredActivities(a => a.IsCompleted && a.AppUserId == userId)
                    .Skip(skipNumber)
                    .Take(pageSize)
                    .ToList();            
        }

        public int GetPreviousActivitiesCount(DateTime now, string? userId)
        {
            return GetFilteredActivities(a => a.IsCompleted && a.AppUserId == userId)
                    .Count();
        }
        #endregion

        #region Stats
        public List<StatisticsViewModel> GetEachActivityCount(string? userId)
        {
            return _dbContext.Activities
                    .Where(a => a.AppUserId == userId)
                    .GroupBy(a => a.ActivityTitle)
                    .Select(g => new StatisticsViewModel
                    {
                        Label = g.Key.Title,
                        Count = g.Count()
                    })
                    .ToList();
        }

        public List<StatisticsViewModel> GetEachActivityTime(string? userId)
        {
            return _dbContext.Activities
                    .Where(a => a.AppUserId == userId)
                    .GroupBy(a => a.ActivityTitle)
                    .AsEnumerable()
                    .Select(g => new StatisticsViewModel
                    {
                        Label = g.Key.Title,
                        Count = CalculateHours(g.Sum(a => a.DurationPlanned.Ticks))
                    })
                    .ToList();
        }
        #endregion

        public void UpdateActivitiesCompletedStatus(DateTime now, string? userId)
        {
            var incompletedActivities = _dbContext.Activities
                            .Where(a => !a.IsCompleted && a.AppUserId == userId)
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

        private double CalculateHours(long time)
        {
            var totalHours = TimeSpan.FromTicks(time).TotalHours;
            var hoursPart = Math.Floor(totalHours);
            var minutesPart = totalHours - hoursPart;
            if (minutesPart > 0)
            {
                minutesPart *= 100;
                minutesPart = Math.Floor(minutesPart);
                minutesPart /= 100;
            }

            return hoursPart + minutesPart;
        }

        public void AddActivity(Activity activity)
        {
            _dbContext.Activities.Add(activity);
            _dbContext.SaveChanges();
        }

        public Activity? GetActivity(int? id)
        {
            return _dbContext.Activities
                             .Include(t => t.ActivityTitle)
                             .Include(c => c.ActivityCategory)
                             .FirstOrDefault(x => x.Id == id);
        }

        public void RemoveActivity(Activity activity)
        {
            _dbContext.Activities.Remove(activity);
            _dbContext.SaveChanges();
        }

        public void UpdateActivity(Activity activity)
        {
            _dbContext.Activities.Update(activity);
            _dbContext.SaveChanges();
        }

        public IQueryable<Activity> GetActivitiesBetweenDays(DateTime startOfWeek, int daysToShow, string? userId)
        {
            return GetFilteredActivities(a => a.StartedAt >= startOfWeek && a.StartedAt < startOfWeek.AddDays(daysToShow) && a.AppUserId == userId);
        }
    }
}
