using JournalToDoMix.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JournalToDoMix.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(AppUser user, string role) => await _userManager.AddToRoleAsync(user, role);        

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password) => await _userManager.CreateAsync(user, password);        

        public async Task<AppUser?> FindUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<AppUser?> FindUserByNameAsync(string name) => await _userManager.FindByNameAsync(name);        

        public async Task<AppUser?> GetCurrentUserAsync(ClaimsPrincipal user) => await _userManager.GetUserAsync(user);        

        public async Task<AppUser?> GetUserByMailAsync(string email) => await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);        

        public async Task SignInAsync(AppUser user, bool isPersistent) => await _signInManager.SignInAsync(user, isPersistent);        

        public Task<SignInResult> SignInWithPasswordAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure) => _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);        

        public async Task SignOutAsync() => await _signInManager.SignOutAsync();        
    }
}
