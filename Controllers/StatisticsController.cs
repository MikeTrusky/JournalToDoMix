using JournalToDoMix.Services;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class StatisticsController : Controller
    {        
        private readonly IActivitiesServices _activitiesServices;
        private readonly IUserService _userService;
        public StatisticsController(IActivitiesServices activitiesServices, IUserService userService)
        {            
            _activitiesServices = activitiesServices;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(ActivitiesCountChart));
        }
        public async Task<IActionResult> ActivitiesCountChart()
        {            
            var user = await _userService.GetCurrentUserAsync(User);
            var eachActivityCount = _activitiesServices.GetEachActivityCount(user?.Id);
            return View(eachActivityCount);
        }
        public async Task<IActionResult> ActivitiesTimeChart()
        {
            var user = await _userService.GetCurrentUserAsync(User);
            var eachActivityTime = _activitiesServices.GetEachActivityTime(user?.Id);
            return View(eachActivityTime);
        }        
    }
}