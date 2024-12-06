using System.ComponentModel.DataAnnotations;

namespace JournalToDoMix.Models
{
    public class ActivityTitle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Description { get; set; }

        
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
