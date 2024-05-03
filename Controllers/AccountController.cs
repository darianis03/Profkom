using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profkom.Data;
using Profkom.Models;

namespace Profkom.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<AccountController> logger
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                viewModel.Email!,
                viewModel.Password!,
                viewModel.RememberMe,
                false
            );

            if (result.Succeeded)
            {
                // Get User 
                var user = await _userManager.GetUserAsync(User);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(viewModel);
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "The password and confirmation password do not match.");
                return View(model);
            }

            var user = new AppUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult ChangeMail()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangeMail(ChangeMailViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user != null)
            {
                user.Email = model.NewEmail;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                    
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
            }
        }
        
        return View(model);
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);

        if (user == null) return RedirectToAction("Login");

        var changePasswordResult = await _userManager.ChangePasswordAsync(
            user,
            model.OldPassword!,
            model.NewPassword!
        );

        if (changePasswordResult.Succeeded) return RedirectToAction("Profile", "Account");
        
        return View(model);
    }
    
    [Authorize]
    public IActionResult Profile()
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            var profileViewModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return View(profileViewModel);
        }

        return RedirectToAction("Error", "Home");
    }
}