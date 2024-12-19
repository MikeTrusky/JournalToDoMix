using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JournalToDoMix.Models
{
    public class ActivityCategory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [DisplayName("Category")]
        public string CategoryName { get; set; } = string.Empty;


        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
