using JournalToDoMix.Models;
using JournalToDoMix.Services;
using JournalToDoMix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JournalToDoMix.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
                return View(loginModel);
                        
            var user = await _userService.FindUserByEmailAsync(loginModel.Email.ToLower());
            if (user == null)
            {
                ModelState.AddModelError("loginError", "Invalid login attempt");
                return View(loginModel);
            }

            var signInResult = await _userService.SignInWithPasswordAsync(user, loginModel.Password, loginModel.RememberMe, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("loginError", "Invalid login attempt");
                return View(loginModel);
            }

            return RedirectToAction("Index", "Timeline");
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
                return View(registerModel);

            if (await UserExistsAsync(registerModel.UserName, registerModel.Email))
                return View(registerModel);

            var appUser = new AppUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email
            };
            
            var createUserResult = await _userService.CreateUserAsync(appUser, registerModel.Password);
            if (!createUserResult.Succeeded)
            {
                AddErrors(createUserResult, "registerError");
                return View(registerModel);
            }
            
            var roleResult = await _userService.AddUserToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                AddErrors(roleResult, "registerError");
                return View(registerModel);
            }
            
            await _userService.SignInAsync(appUser, isPersistent: false);
            return RedirectToAction("Index", "Timeline");
        }

        private async Task<bool> UserExistsAsync(string userName, string email)
        {
            bool error = false;
            if (await _userService.FindUserByNameAsync(userName) != null)
            {
                ModelState.AddModelError("UserName", "This username is already taken.");
                error = true;
            }

            if (await _userService.FindUserByEmailAsync(email) != null)
            {
                ModelState.AddModelError("Email", "This email is already taken.");
                error = true;
            }            

            return error;
        }

        private void AddErrors(IdentityResult result, string modelErrorKey)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(modelErrorKey, error.Description);
            }
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Index", "Timeline");
        }
    }
}
