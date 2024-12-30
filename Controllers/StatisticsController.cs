using JournalToDoMix.Services;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class StatisticsController : Controller
    {        
        private readonly IActivitiesServices _activitiesServices;
        public StatisticsController(IActivitiesServices activitiesServices)
        {            
            _activitiesServices = activitiesServices;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(ActivitiesCountChart));
        }
        public IActionResult ActivitiesCountChart()
        {
            var eachActivityCount = _activitiesServices.GetEachActivityCount();
            return View(eachActivityCount);
        }
        public IActionResult ActivitiesTimeChart()
        {
            var eachActivityTime = _activitiesServices.GetEachActivityTime();
            return View(eachActivityTime);
        }        
    }
}
