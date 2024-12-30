using JournalToDoMix.Models;
using JournalToDoMix.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public StatisticsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(ActivitiesCountChart));
        }
        public IActionResult ActivitiesCountChart()
        {
            var eachActivityCount = _dbContext.Activities
                                                .GroupBy(a => a.ActivityTitle)
                                                .Select(g => new StatisticsViewModel
                                                {
                                                    Label = g.Key.Title,
                                                    Count = g.Count(),
                                                    Type = ChartType.ActivityCount
                                                })
                                                .ToList();

            return View(eachActivityCount);
        }
        public IActionResult ActivitiesTimeChart()
        {
            var eachActivityTime = _dbContext.Activities
                                                .GroupBy(a => a.ActivityTitle)         
                                                .AsEnumerable()
                                                .Select(g => new StatisticsViewModel
                                                {
                                                    Label = g.Key.Title,
                                                    Count = CalculateHours(g.Sum(a => a.DurationPlanned.Ticks)),
                                                    Type = ChartType.ActivityTime
                                                })
                                                .ToList();

            return View(eachActivityTime);
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
    }
}
