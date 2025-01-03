using JournalToDoMix.Models;
using JournalToDoMix.ViewModels;
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginModel.Email.ToLower());
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(loginModel);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);

            if(result.Succeeded)
            {                
                return RedirectToAction("Index", "Timeline");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(loginModel);
            }            
        }
        public IActionResult Register()
        {                          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
                return View(registerModel);

            var appUser = new AppUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email                
            };

            var createUser = await _userManager.CreateAsync(appUser, registerModel.Password);

            if (createUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    return RedirectToAction("Index", "Timeline");
                }                
                else
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            foreach(var error in createUser.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerModel);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Timeline");
        }
    }
}
