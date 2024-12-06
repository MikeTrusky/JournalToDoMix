using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace JournalToDoMix.ViewModels
{
    public class ActivityEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        public string? Description { get; set; }        

        [Required(ErrorMessage = "Started time is required")]
        [DisplayName("Started at")]
        public DateTime StartedAt { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [DisplayName("Duration")]
        public TimeSpan DurationPlanned { get; set; }
    }
}
