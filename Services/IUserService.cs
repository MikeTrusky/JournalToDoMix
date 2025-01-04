using JournalToDoMix.Models;
using System.Security.Claims;

namespace JournalToDoMix.Services
{
    public interface IUserService
    {
        Task<AppUser?> GetCurrentUserAsync(ClaimsPrincipal user);
    }
}
