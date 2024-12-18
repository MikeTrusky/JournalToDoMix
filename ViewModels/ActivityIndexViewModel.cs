using JournalToDoMix.Models;

namespace JournalToDoMix.ViewModels
{
    public class ActivityIndexViewModel
    {
        public string ActivityType { get; set; } = "Current";
        public bool IsCurrent { get; set; } = true;
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
