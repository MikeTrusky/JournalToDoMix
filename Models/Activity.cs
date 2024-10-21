using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JournalToDoMix.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Title { get; set; }       
        public string Description { get; set; }
        [DisplayName("Is completed")]
        public bool IsCompleted { get; set; }
        [DisplayName("Started at")]
        public DateTime StartedAt { get; set; }
        [DisplayName("Duration planned")]
        public TimeSpan DurationPlanned { get; set; }
    }
}
