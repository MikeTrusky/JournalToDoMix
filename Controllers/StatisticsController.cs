using JournalToDoMix.Models;
using JournalToDoMix.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class StatisticsController : Controller
    {        
        private readonly IActivitiesServices _activitiesServices;
        private readonly UserManager<AppUser> _userManager;
        public StatisticsController(IActivitiesServices activitiesServices, UserManager<AppUser> userManager)
        {            
            _activitiesServices = activitiesServices;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction(nameof(ActivitiesCountChart));
        }
        public async Task<IActionResult> ActivitiesCountChart()
        {
            var user = await _userManager.GetUserAsync(User);
            var eachActivityCount = _activitiesServices.GetEachActivityCount(user?.Id);
            return View(eachActivityCount);
        }
        public async Task<IActionResult> ActivitiesTimeChart()
        {
            var user = await _userManager.GetUserAsync(User);
            var eachActivityTime = _activitiesServices.GetEachActivityTime(user?.Id);
            return View(eachActivityTime);
        }        
    }
}
