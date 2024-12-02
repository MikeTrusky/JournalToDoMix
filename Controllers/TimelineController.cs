using JournalToDoMix.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace JournalToDoMix.Controllers
{
    public class TimelineController : Controller
    {
        private readonly ILogger<TimelineController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public TimelineController(ILogger<TimelineController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public IActionResult Index(string? startOfWeek, int daysOfWeekToAdd = 0, int daysToShow = 7)
        {
            DateTime startOfCurrentWeek;
            if(ModelState.IsValid && !string.IsNullOrEmpty(startOfWeek) && DateTime.TryParse(startOfWeek, CultureInfo.InvariantCulture, out var parsedData))
            {
                startOfCurrentWeek = parsedData;                
            }
            else
            {
                startOfCurrentWeek = GetStartOfCurrentWeek();
            }

            daysOfWeekToAdd *= daysToShow;
            startOfCurrentWeek = startOfCurrentWeek.AddDays(daysOfWeekToAdd);

            ViewBag.DaysOfWeek = GetDaysOfWeek(startOfCurrentWeek, daysToShow);
            ViewBag.EarliestActivitiesByDay = GetEarliestActivitiesForWeek(startOfCurrentWeek, ViewBag.DaysOfWeek, daysToShow);

            return View();
        }
        private List<DateTime> GetDaysOfWeek(DateTime startOfWeek, int daysToShow)
        {
            return Enumerable.Range(0, daysToShow)
                             .Select(offset => startOfWeek.AddDays(offset))
                             .ToList();
        }
        private Dictionary<DateTime, Activity?> GetEarliestActivitiesForWeek(DateTime startOfWeek, List<DateTime> daysOfWeek, int daysToShow)
        {
            var activities = _dbContext.Activities
                                       .Where(a => a.StartedAt >= startOfWeek && a.StartedAt < startOfWeek.AddDays(daysToShow))
                                       .ToList();

            return daysOfWeek.ToDictionary(
                   day => day,
                   day => activities.Where(a => a.StartedAt.Date == day.Date)
                                    .OrderBy(a => a.StartedAt)
                                    .FirstOrDefault());
        }
        private DateTime GetStartOfCurrentWeek()
        {
            var now = DateTime.Now;
            return now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
        }
    }
}
