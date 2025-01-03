using JournalToDoMix.Models;
using JournalToDoMix.Services;
using JournalToDoMix.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ILogger<ActivitiesController> _logger;
        private readonly IActivitiesServices _activitiesServices;
        private readonly IActivitiesTitlesServices _activitiesTitlesServices;
        private readonly IActivitiesCategoriesServices _activitiesCategoriesServices;

        public ActivitiesController(ILogger<ActivitiesController> logger, 
                                    IActivitiesServices activitiesServices, 
                                    IActivitiesTitlesServices activitiesTitlesServices,
                                    IActivitiesCategoriesServices activitiesCategoriesServices)
        {
            _logger = logger;
            _activitiesServices = activitiesServices;
            _activitiesTitlesServices = activitiesTitlesServices;
            _activitiesCategoriesServices = activitiesCategoriesServices;
        }
        #region Index
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
                    "Planned" => _activitiesServices.GetPlannedActivities(now, activityIndexViewModel.PageSize, activityIndexViewModel.PageNumber),
                    "Current" => _activitiesServices.GetCurrentActivities(now, activityIndexViewModel.PageSize, activityIndexViewModel.PageNumber),
                    "Previous" => _activitiesServices.GetPreviousActivities(now, activityIndexViewModel.PageSize, activityIndexViewModel.PageNumber),
                    _ => throw new InvalidOperationException()
                };
                activityIndexViewModel.AllPagesNumber = activityIndexViewModel.ActivityType switch
                {
                    "Planned" => (int)Math.Ceiling((float)_activitiesServices.GetPlannedActivitiesCount(now) / activityIndexViewModel.PageSize),
                    "Current" => (int)Math.Ceiling((float)_activitiesServices.GetCurrentActivitiesCount(now) / activityIndexViewModel.PageSize),
                    "Previous" => (int)Math.Ceiling((float)_activitiesServices.GetPreviousActivitiesCount(now) / activityIndexViewModel.PageSize),
                    _ => throw new InvalidOperationException()
                };
            }
            catch(InvalidOperationException)
            {
                return RedirectToAction("Index");
            }

            if (activityIndexViewModel.AllPagesNumber == 0)
                activityIndexViewModel.AllPagesNumber = 1;

            activityIndexViewModel.IsCurrent = (activityIndexViewModel.ActivityType == "Current");

            return View(activityIndexViewModel);
        }
        #endregion
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

            var title = _activitiesTitlesServices.FindTitle(activityViewModel.Title);
            if (title == null)
            {
                title = new ActivityTitle
                {
                    Title = activityViewModel.Title,
                    Description = activityViewModel.Description
                };
                _activitiesTitlesServices.AddTitle(title);                
            }

            var category = _activitiesCategoriesServices.FindCategory(activityViewModel.Category);
            if (category == null)
            {
                category = new ActivityCategory
                {
                    CategoryName = activityViewModel.Category
                };
                _activitiesCategoriesServices.AddCategory(category);
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

            _activitiesServices.AddActivity(activity);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetActivityTitles(string query)
        {
            var titles = await _activitiesTitlesServices.GetTitlesStartsWithAsync(query);
            return Json(titles);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _activitiesCategoriesServices.GetCategoriesNamesAsync();
            return Json(categories);
        }
        #endregion
        #region Edit action
        public IActionResult Edit(int? id)
        {
            var activity = _activitiesServices.GetActivity(id);

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

            var activity = _activitiesServices.GetActivity(activityViewModel.Id);

            if (activity == null)
                return NotFound();
            
            var title = _activitiesTitlesServices.FindTitle(activityViewModel.Title);
            if (title == null)
            {
                title = new ActivityTitle
                {
                    Title = activityViewModel.Title,
                    Description = activityViewModel.Description
                };
                _activitiesTitlesServices.AddTitle(title);                
            }

            var category = _activitiesCategoriesServices.FindCategory(activityViewModel.Category);            
            if (category == null)
            {
                category = new ActivityCategory
                {
                    CategoryName = activityViewModel.Category
                };
                _activitiesCategoriesServices.AddCategory(category);
            }

            activity.ActivityTitle = title;
            activity.ActivityCategory = category;
            activity.StartedAt = activityViewModel.StartedAt;
            activity.DurationPlanned = activityViewModel.DurationPlanned;
            activity.IsCompleted = DateTime.Now > activity.StartedAt.Add(activity.DurationPlanned);

            if (activityViewModel.Description != null && activityViewModel.Description != title.Description)
                activity.Description = activityViewModel.Description;

            _activitiesServices.UpdateActivity(activity);

            return RedirectToAction("Index");
        }
        #endregion
        #region Delete action
        public IActionResult Delete(int? id)
        {
            var activity = _activitiesServices.GetActivity(id);            
            if (activity != null)
            {
                _activitiesServices.RemoveActivity(activity);                
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
