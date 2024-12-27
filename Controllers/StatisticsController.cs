using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
