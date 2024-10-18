using JournalToDoMix.Models;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class ActionsController : Controller
    {
        private readonly ILogger<ActionsController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ActionsController(ILogger<ActionsController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var actions = _dbContext.Actions.ToList();
            return View(actions);
        }
    }
}
