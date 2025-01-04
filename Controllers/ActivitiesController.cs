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
        private readonly IUserService _userService;

        public ActivitiesController(ILogger<ActivitiesController> logger, 
                                    IActivitiesServices activitiesServices, 
                                    IActivitiesTitlesServices activitiesTitlesServices,
                                    IActivitiesCategoriesServices activitiesCategoriesServices,
                                    IUserService userService)
        {
            _logger = logger;
            _activitiesServices = activitiesServices;
            _activitiesTitlesServices = activitiesTitlesServices;
            _activitiesCategoriesServices = activitiesCategoriesServices;
            _userService = userService;
        }
        #region Index
        public async Task<IActionResult> Index(ActivityIndexViewModel activityIndexViewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            activityIndexViewModel ??= new ActivityIndexViewModel();

            var now = DateTime.Now;

            var user = await _userService.GetCurrentUserAsync(User);

            _activitiesServices.UpdateActivitiesCompletedStatus(now, user?.Id);

            try
            {
                activityIndexViewModel.Activities = activityIndexViewModel.ActivityType switch
                {
                    "Planned" => _activitiesServices.GetPlannedActivities(now, activityIndexViewModel.PageSize, activityIndexViewModel.PageNumber, user?.Id),
                    "Current" => _activitiesServices.GetCurrentActivities(now, activityIndexViewModel.PageSize, activityIndexViewModel.PageNumber, user?.Id),
                    "Previous" => _activitiesServices.GetPreviousActivities(now, activityIndexViewModel.PageSize, activityIndexViewModel.PageNumber, user?.Id),
                    _ => throw new InvalidOperationException()
                };
                activityIndexViewModel.AllPagesNumber = activityIndexViewModel.ActivityType switch
                {
                    "Planned" => (int)Math.Ceiling((float)_activitiesServices.GetPlannedActivitiesCount(now, user?.Id) / activityIndexViewModel.PageSize),
                    "Current" => (int)Math.Ceiling((float)_activitiesServices.GetCurrentActivitiesCount(now, user?.Id) / activityIndexViewModel.PageSize),
                    "Previous" => (int)Math.Ceiling((float)_activitiesServices.GetPreviousActivitiesCount(now, user?.Id) / activityIndexViewModel.PageSize),
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
        public async Task<IActionResult> Add(ActivityCreateViewModel activityViewModel)
        {
            if (!ModelState.IsValid)
                return View(activityViewModel);            

            var user = await _userService.GetCurrentUserAsync(User);
            var title = _activitiesTitlesServices.GetTitle(activityViewModel.Title, activityViewModel.Description);
            var category = _activitiesCategoriesServices.GetCategory(activityViewModel.Category);

            var activity = new Activity
            {
                ActivityTitle = title,
                ActivityCategory = category,
                StartedAt = activityViewModel.StartedAt,
                DurationPlanned = activityViewModel.DurationPlanned,
                IsCompleted = DateTime.Now > activityViewModel.StartedAt.Add(activityViewModel.DurationPlanned),
                AppUser = user
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
        public async Task<IActionResult> Edit(ActivityEditViewModel activityViewModel)
        {
            if (!ModelState.IsValid)
                return View(activityViewModel);

            var activity = _activitiesServices.GetActivity(activityViewModel.Id);

            if (activity == null)
                return NotFound();                       

            var user = await _userService.GetCurrentUserAsync(User);
            var title = _activitiesTitlesServices.GetTitle(activityViewModel.Title, activityViewModel.Description);
            var category = _activitiesCategoriesServices.GetCategory(activityViewModel.Category);

            activity.ActivityTitle = title;
            activity.ActivityCategory = category;
            activity.StartedAt = activityViewModel.StartedAt;
            activity.DurationPlanned = activityViewModel.DurationPlanned;
            activity.IsCompleted = DateTime.Now > activity.StartedAt.Add(activity.DurationPlanned);
            activity.AppUser = user;

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
                _activitiesServices.RemoveActivity(activity);

            return RedirectToAction("Index");
        }
        #endregion        
    }
}
