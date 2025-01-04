using JournalToDoMix.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JournalToDoMix.Services
{
    public interface IUserService
    {
        Task<AppUser?> GetCurrentUserAsync(ClaimsPrincipal user);        
        Task<SignInResult> SignInWithPasswordAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure);
        Task<IdentityResult> CreateUserAsync(AppUser user, string password);
        Task<IdentityResult> AddUserToRoleAsync(AppUser user, string role);
        Task SignInAsync(AppUser user, bool isPersistent);
        Task<AppUser?> FindUserByNameAsync(string name);
        Task<AppUser?> FindUserByEmailAsync(string email);
        Task SignOutAsync();
    }
}
