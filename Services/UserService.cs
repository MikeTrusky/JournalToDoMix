using JournalToDoMix.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JournalToDoMix.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager) 
        { 
            _userManager = userManager;
        }
        public async Task<AppUser?> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return user == null ? null : await _userManager.GetUserAsync(user);
        }
    }
}
