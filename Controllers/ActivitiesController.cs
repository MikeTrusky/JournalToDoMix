using JournalToDoMix.Models;
using JournalToDoMix.Services;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ILogger<ActivitiesController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IActivitiesServices _activitiesServices;

        public ActivitiesController(ILogger<ActivitiesController> logger, ApplicationDbContext dbContext, IActivitiesServices activitiesServices)
        {
            _logger = logger;
            _dbContext = dbContext;
            _activitiesServices = activitiesServices;
        }
        public IActionResult Index()
        {
            var now = DateTime.Now;

            _activitiesServices.UpdateActivitiesCompletedStatus(now);

            ViewBag.PlannedActivities = _activitiesServices.GetPlannedActivities(now);
            ViewBag.CurrentActivities = _activitiesServices.GetCurrentActivities(now);
            ViewBag.PreviousActivities = _activitiesServices.GetPreviousActivities(now);

            return View();
        }
        #region Add new actions
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Activities.Add(activity);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(activity);
        }
        #endregion
        #region Edit action
        public IActionResult Edit(int? id)
        {
            var activity = _dbContext.Activities.FirstOrDefault(x => x.Id == id);
            return View(activity);
        }
        [HttpPost]
        public IActionResult Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Activities.Update(activity);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(activity);
        }
        #endregion
        #region Delete action
        public IActionResult Delete(int? id)
        {
            var activity = _dbContext.Activities.FirstOrDefault(x => x.Id == id);
            if(activity != null)
            {
                _dbContext.Activities.Remove(activity);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
