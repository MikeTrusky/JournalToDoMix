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
        public IActionResult Index(string activitiesType= "Current Activities")
        {
            var now = DateTime.Now;

            _activitiesServices.UpdateActivitiesCompletedStatus(now);

            switch (activitiesType)
            {
                case "Planned Activities":
                    ViewBag.Current = false;
                    ViewBag.Activities = _activitiesServices.GetPlannedActivities(now);
                    break;
                case "Current Activities":                    
                    ViewBag.Current = true;
                    ViewBag.Activities = _activitiesServices.GetCurrentActivities(now);
                    break;
                case "Previous Activities":
                    ViewBag.Current = false;
                    ViewBag.Activities = _activitiesServices.GetPreviousActivities(now);
                    break;
            }

            ViewBag.Caption = activitiesType;

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
