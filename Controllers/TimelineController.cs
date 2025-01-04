using JournalToDoMix.Models;
using JournalToDoMix.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace JournalToDoMix.Controllers
{
    public class TimelineController : Controller
    {
        private readonly ILogger<TimelineController> _logger;
        private readonly IActivitiesServices _activitiesServices;
        private readonly IUserService _userService;

        public TimelineController(ILogger<TimelineController> logger, IActivitiesServices activitiesServices, IUserService userService)
        {
            _logger = logger;
            _activitiesServices = activitiesServices;
            _userService = userService;
        }
        public async Task<IActionResult> Index(string? startOfWeek, int daysOfWeekToAdd = 0, int daysToShow = 7)
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
            ViewBag.ActivitiesByDay = await GetActivitiesBetweenDaysAsync(startOfCurrentWeek, ViewBag.DaysOfWeek, daysToShow);

            return View();
        }
        private List<DateTime> GetDaysOfWeek(DateTime startOfWeek, int daysToShow)
        {
            return Enumerable.Range(0, daysToShow)
                             .Select(offset => startOfWeek.AddDays(offset))
                             .ToList();
        }
        private async Task<Dictionary<DateTime, List<Activity>>> GetActivitiesBetweenDaysAsync(DateTime startOfWeek, List<DateTime> daysOfWeek, int daysToShow)
        {            
            var user = await _userService.GetCurrentUserAsync(User);
            var activities = _activitiesServices.GetActivitiesBetweenDays(startOfWeek, daysToShow, user?.Id);

            return daysOfWeek.ToDictionary(
                   day => day,
                   day => activities.Where(a => a.StartedAt.Date == day.Date)
                                    .OrderBy(a => a.StartedAt)
                                    .ToList());
        }
        private DateTime GetStartOfCurrentWeek()
        {
            var now = DateTime.Now;
            return now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
        }
    }
}
