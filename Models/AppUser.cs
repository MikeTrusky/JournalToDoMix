using Microsoft.AspNetCore.Identity;

namespace JournalToDoMix.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
