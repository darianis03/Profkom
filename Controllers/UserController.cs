using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Profkom.Data;
using Profkom.Models;

namespace Profkom.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public UserController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        ILogger<UserController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddUserRole(ModifyUserViewModel model)
    {
        if (!string.IsNullOrEmpty(model.SelectedRole))
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                // Handle user not found
                var userErrorMessage = $"User not found with ID: {model.UserId}";
                _logger.LogWarning(userErrorMessage);
                return NotFound(userErrorMessage);
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.SelectedRole);

            if (!roleExists)
            {
                // Handle role not found
                ModelState.AddModelError("", "Role does not exist.");
                return View("ModifyUser", model); // Redirect to ModifyUser view with the model
            }

            var result = await _userManager.AddToRoleAsync(user, model.SelectedRole);
            if (result.Succeeded)
            {
                // Role added successfully

                // Reload user roles after adding a new role
                var updatedUserRoles = await _userManager.GetRolesAsync(user);

                // Update the model with the new roles
                model.Roles = updatedUserRoles;

                var successMessage = $"Role '{model.SelectedRole}' added to user '{user.UserName}' successfully.";
                _logger.LogInformation(successMessage);

                // Redirect to the ModifyUser view with the complete userId parameter
                return RedirectToAction("ModifyUser", new { userId = model.UserId });
            }

            // Handle error adding role
            var addRoleErrorMessage =
                $"Failed to add role '{model.SelectedRole}' to user '{user.UserName}'. Errors: {string.Join(", ", result.Errors)}";
            _logger.LogError(addRoleErrorMessage);
            ModelState.AddModelError("", "Failed to add role.");
        }

        // Handle invalid role selection or other issues
        return View("ModifyUser", model); // Redirect to ModifyUser view with the model
    }
    
    [HttpPost]
    public IActionResult RemoveUserRole(string userId, string roleName)
    {
        // Fetch the user based on the userId
        var user = _userManager.FindByIdAsync(userId).Result;

        if (user == null)
            // Handle user not found
            return NotFound();

        // Check if the user has the role
        var isInRole = _userManager.IsInRoleAsync(user, roleName).Result;
        if (!isInRole)
            // Handle the case where the user doesn't have the role
            return NotFound();

        // Remove the role from the user
        var result = _userManager.RemoveFromRoleAsync(user, roleName).Result;
        if (result.Succeeded)
            // Role removed successfully
            // Redirect to the modify user view or perform any necessary actions
            return RedirectToAction("ModifyUser", new { userId });

        // Handle the case where role removal fails
        // You can redirect to an error page or perform other error handling
        return View("ModifyUser");
    }
    
    [HttpPost]
    public IActionResult UpdateUser(ModifyUserViewModel model)
    {
        if (!ModelState.IsValid)
            // If the model state is not valid, return the view with validation errors
            return View("ModifyUser", model);

        // Fetch the user based on the model's UserId
        var user = _userManager.FindByIdAsync(model.UserId).Result;

        if (user == null)
            // Handle user not found
            return NotFound();

        // Update user details
        user.UserName = model.UserName;
        user.PhoneNumber = model.PhoneNumber;
        user.Email = model.Email;

        var result = _userManager.UpdateAsync(user).Result;
        if (result.Succeeded)
            // User details updated successfully
            // Redirect to the ManageUsers view or perform any necessary actions
            return RedirectToAction("ModifyUser", new { userId = model.UserId });

        // Handle the case where user update fails
        // You can redirect to an error page or perform other error handling
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null) return NotFound();

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
            // Redirect to ManageUsers after successful deletion
            return RedirectToAction("ManageUsers");

        // Handle errors if the deletion fails
        ModelState.AddModelError("", "Failed to delete the user.");
        return View("ManageUsers");
    }
    
    public IActionResult ModifyUser(string userId)
    {
        var user = _userManager.FindByIdAsync(userId).Result;

        if (user == null)
            // Handle user not found
            return NotFound();

        // Retrieve user roles
        var userRoles = _userManager.GetRolesAsync(user).Result;
        foreach (var roleName in userRoles) Console.WriteLine(roleName);

        var availableRoles = _roleManager.Roles.ToList();

        // Pass user data and roles to the ModifyUser view
        var model = new ModifyUserViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Roles = userRoles,
            AvailableRoles = availableRoles
        };

        return View(model);
    }
    
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        // Retrieve user roles asynchronously
        var usersRoles = new Dictionary<string, List<string>>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            usersRoles[user.Id] = roles.ToList();
        }

        var model = new ManageUsersViewModel
        {
            Users = users,
            UserRoles = usersRoles
        };

        return View(model);
    }
}