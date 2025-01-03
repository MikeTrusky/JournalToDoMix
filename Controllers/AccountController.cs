using JournalToDoMix.Models;
using JournalToDoMix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JournalToDoMix.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginModel.Email.ToLower());
            if (user == null)
            {
                ModelState.AddModelError("loginError", "Invalid login attempt");
                return View(loginModel);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);

            if (!result.Succeeded)
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

            if (await UserExists(registerModel.UserName, registerModel.Email))
                return View(registerModel);

            var appUser = new AppUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email
            };

            var createUserResult = await _userManager.CreateAsync(appUser, registerModel.Password);

            if (!createUserResult.Succeeded)
            {
                AddErrors(createUserResult, "registerError");
                return View(registerModel);
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (!roleResult.Succeeded)
            {
                AddErrors(roleResult, "registerError");
                return View(registerModel);
            }

            await _signInManager.SignInAsync(appUser, isPersistent: false);
            return RedirectToAction("Index", "Timeline");
        }

        private async Task<bool> UserExists(string userName, string email)
        {
            bool error = false;
            if (await _userManager.FindByNameAsync(userName) != null)
            {
                ModelState.AddModelError("UserName", "This username is already taken.");
                error = true;
            }

            if (await _userManager.FindByEmailAsync(email) != null)
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
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Timeline");
        }
    }
}
