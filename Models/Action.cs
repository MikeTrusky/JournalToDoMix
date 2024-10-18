using System.ComponentModel.DataAnnotations;

namespace JournalToDoMix.Models
{
    public class Action
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Title { get; set; }       
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime StartedAt { get; set; }
        public TimeSpan DurationPlanned { get; set; }
    }
}
