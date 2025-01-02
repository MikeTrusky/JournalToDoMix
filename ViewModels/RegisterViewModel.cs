using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JournalToDoMix.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]        
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
