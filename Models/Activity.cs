using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JournalToDoMix.Models
{
    public class Activity
    {
        public int Id { get; set; }     
        public string? Description { get; set; }
        [DisplayName("Is completed")]
        public bool IsCompleted { get; set; }
        [Required]
        [DisplayName("Started at")]
        public DateTime StartedAt { get; set; }
        [Required]
        [DisplayName("Duration")]
        public TimeSpan DurationPlanned { get; set; }

        public int ActivityTitleId { get; set; }
        public ActivityTitle ActivityTitle { get; set; } = null!;

        public int ActivityCategoryId { get; set; }
        public ActivityCategory ActivityCategory { get; set;} = null!;

        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
