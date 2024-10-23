using JournalToDoMix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ILogger<ActivitiesController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ActivitiesController(ILogger<ActivitiesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var now = DateTime.Now;

            UpdateActivitiesCompletedStatus(now);

            var plannedActivities = _dbContext.Activities
                .Where(a => a.StartedAt > now)
                .AsNoTracking()
                .ToList();

            var currentActivities = _dbContext.Activities
                .Where(a => a.StartedAt <= now && !a.IsCompleted)
                .AsNoTracking()
                .AsEnumerable()
                .Where(a => a.StartedAt.Add(a.DurationPlanned) >= now)
                .ToList();

            var previousActivities = _dbContext.Activities
                .Where(a => a.IsCompleted ||
                            (!plannedActivities.Select(p => p.Id).Contains(a.Id) &&
                             !currentActivities.Select(c => c.Id).Contains(a.Id)))
                .AsNoTracking()
                .ToList();

            ViewBag.PlannedActivities = plannedActivities;
            ViewBag.CurrentActivities = currentActivities;
            ViewBag.PreviousActivities = previousActivities;

            return View();
        }

        private void UpdateActivitiesCompletedStatus(DateTime now)
        {
            var incompletedActivities = _dbContext.Activities
                            .Where(a => !a.IsCompleted)
                            .ToList();

            bool hasChange = false;

            foreach (var activity in incompletedActivities)
            {
                var endTime = activity.StartedAt.Add(activity.DurationPlanned);
                if (endTime < now)
                {
                    activity.IsCompleted = true;
                    _dbContext.Activities.Update(activity);
                    hasChange = true;
                }
            }

            if (hasChange)
                _dbContext.SaveChanges();
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
