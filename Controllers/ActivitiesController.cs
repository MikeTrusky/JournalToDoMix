using JournalToDoMix.Models;
using JournalToDoMix.Services;
using JournalToDoMix.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index(ActivityIndexViewModel activityIndexViewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            activityIndexViewModel ??= new ActivityIndexViewModel();

            var now = DateTime.Now;

            _activitiesServices.UpdateActivitiesCompletedStatus(now);

            try
            {
                activityIndexViewModel.Activities = activityIndexViewModel.ActivityType switch
                {
                    "Planned" => _activitiesServices.GetPlannedActivities(now),
                    "Current" => _activitiesServices.GetCurrentActivities(now),
                    "Previous" => _activitiesServices.GetPreviousActivities(now),
                    _ => throw new InvalidOperationException()
                };
            }
            catch(InvalidOperationException ex)
            {
                return RedirectToAction("Index");
            }            

            activityIndexViewModel.IsCurrent = (activityIndexViewModel.ActivityType == "Current");

            return View(activityIndexViewModel);
        }
        #region Add new actions
        public IActionResult Add()
        {
            ActivityCreateViewModel activityViewModel = new()
            {
                StartedAt = new DateTime(
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    DateTime.Now.Hour,
                    DateTime.Now.Minute,
                    0,
                    DateTimeKind.Local
                ),
                DurationPlanned = new TimeSpan(0, 10, 0)
            };
            return View(activityViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ActivityCreateViewModel activityViewModel)
        {
            if (!ModelState.IsValid)
                return View(activityViewModel);

            var title = _dbContext.ActivityTitles.FirstOrDefault(x => x.Title == activityViewModel.Title);
            if (title == null)
            {
                title = new ActivityTitle
                {
                    Title = activityViewModel.Title,
                    Description = activityViewModel.Description
                };
                _dbContext.ActivityTitles.Add(title);
            }

            var category = _dbContext.ActivityCategories.FirstOrDefault(x => x.CategoryName == activityViewModel.Category);
            if (category == null)
            {
                category = new ActivityCategory
                {
                    CategoryName = activityViewModel.Category
                };
                _dbContext.ActivityCategories.Add(category);
            }

            var activity = new Activity
            {
                ActivityTitle = title,
                ActivityCategory = category,
                StartedAt = activityViewModel.StartedAt,
                DurationPlanned = activityViewModel.DurationPlanned,
                IsCompleted = DateTime.Now > activityViewModel.StartedAt.Add(activityViewModel.DurationPlanned),
            };

            if (activityViewModel.Description != null && activityViewModel.Description != title.Description)
                activity.Description = activityViewModel.Description;

            _dbContext.Activities.Add(activity);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetActivityTitles(string query)
        {
            var titles = await _dbContext.ActivityTitles
                .Where(t => t.Title.StartsWith(query))
                .Select(t => t.Title)
                .ToListAsync();

            return Json(titles);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _dbContext.ActivityCategories
                .Select(c => c.CategoryName)
                .ToListAsync();

            return Json(categories);
        }
        #endregion
        #region Edit action
        public IActionResult Edit(int? id)
        {
            var activity = _dbContext.Activities
                                     .Include(t => t.ActivityTitle)
                                     .Include(c => c.ActivityCategory)
                                     .FirstOrDefault(x => x.Id == id);
            if (activity == null)
                return NotFound();

            var activityViewModel = new ActivityEditViewModel
            {
                Id = activity.Id,                
                Title = activity.ActivityTitle.Title,
                Description = (activity.Description != null && activity.Description != string.Empty) ? activity.Description : activity.ActivityTitle.Description,
                Category = activity.ActivityCategory.CategoryName,
                StartedAt = activity.StartedAt,
                DurationPlanned = activity.DurationPlanned
            };
            
            return View(activityViewModel);
        }
        [HttpPost]
        public IActionResult Edit(ActivityEditViewModel activityViewModel)
        {
            if (!ModelState.IsValid)
                return View(activityViewModel);

            var activity = _dbContext.Activities
                                     .Include(t => t.ActivityTitle)
                                     .Include(c => c.ActivityCategory)
                                     .FirstOrDefault(x => x.Id == activityViewModel.Id);

            if(activity == null)
                return NotFound();

            var title = _dbContext.ActivityTitles.FirstOrDefault(x => x.Title == activityViewModel.Title);
            if (title == null)
            {
                title = new ActivityTitle
                {
                    Title = activityViewModel.Title,
                    Description = activityViewModel.Description
                };
                _dbContext.ActivityTitles.Add(title);
            }

            var category = _dbContext.ActivityCategories.FirstOrDefault(x => x.CategoryName == activityViewModel.Category);
            if (category == null)
            {
                category = new ActivityCategory
                {
                    CategoryName = activityViewModel.Category
                };
                _dbContext.ActivityCategories.Add(category);
            }            

            activity.ActivityTitle = title;
            activity.ActivityCategory = category;
            activity.StartedAt = activityViewModel.StartedAt;
            activity.DurationPlanned = activityViewModel.DurationPlanned;
            activity.IsCompleted = DateTime.Now > activity.StartedAt.Add(activity.DurationPlanned);

            if (activityViewModel.Description != null && activityViewModel.Description != title.Description)
                activity.Description = activityViewModel.Description;
            
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion
        #region Delete action
        public IActionResult Delete(int? id)
        {
            var activity = _dbContext.Activities.FirstOrDefault(x => x.Id == id);
            if (activity != null)
            {
                _dbContext.Activities.Remove(activity);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
