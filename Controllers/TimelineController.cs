using JournalToDoMix.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var now = DateTime.Now;
            var startOfWeek = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday); // Poniedziałek bieżącego tygodnia
            var daysOfWeek = Enumerable.Range(0, 7)
                                       .Select(offset => startOfWeek.AddDays(offset))
                                       .ToList();


            var activities = _dbContext.Activities
                .Where(a => a.StartedAt >= startOfWeek && a.StartedAt < startOfWeek.AddDays(7))
                .ToList();

            var earliestActivitiesByDay = daysOfWeek.ToDictionary(
                day => day,
                day => activities.Where(a => a.StartedAt.Date == day.Date)
                                 .OrderBy(a => a.StartedAt)
                                 .FirstOrDefault());

            ViewBag.DaysOfWeek = daysOfWeek;
            ViewBag.EarliestActivitiesByDay = earliestActivitiesByDay;

            return View();
        }
    }
}
