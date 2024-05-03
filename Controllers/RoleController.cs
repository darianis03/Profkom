using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profkom.Data;
using Profkom.Models;

namespace Profkom.Controllers;

public class RoleController : Controller
{
    private readonly ILogger<RoleController> _logger;
    private readonly RoleManager<AppRole> _roleManager;

    public RoleController(RoleManager<AppRole> roleManager, ILogger<RoleController> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        try
        {
            // Create a new role object with the provided roleName
            var newRole = new AppRole { Name = roleName };

            // Attempt to create the new role
            var result = await _roleManager.CreateAsync(newRole);

            // If creation succeeded, log success and redirect to ManageRoles action
            if (result.Succeeded)
            {
                _logger.LogInformation($"Role '{newRole.Name}' created successfully.");
                return RedirectToAction("ManageRoles");
            }

            // If creation failed, log errors, add error to ModelState, and return ManageRoles view
            _logger.LogError($"Failed to create role '{newRole.Name}'. Errors: {string.Join(", ", result.Errors)}");
            ModelState.AddModelError("", "Failed to create the role. Please check the provided information.");
            return View("ManageRoles");
        }
        catch (Exception ex)
        {
            // If an exception occurs, log the error and return StatusCode 500
            _logger.LogError(ex, "An error occurred while creating the role.");
            return StatusCode(500, "An error occurred while creating the role.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateRole(string roleId, string newRoleName)
    {
        try
        {
            // Find the role by its ID
            var role = await _roleManager.FindByIdAsync(roleId);

            // If role not found, log a warning and return NotFound result
            if (role == null)
            {
                _logger.LogWarning($"Role with ID '{roleId}' not found.");
                return NotFound();
            }

            // Update the role name
            role.Name = newRoleName;

            // Attempt to update the role
            var result = await _roleManager.UpdateAsync(role);

            // If update succeeded, log success and redirect to ManageRoles action
            if (result.Succeeded)
            {
                _logger.LogInformation($"Role '{role.Name}' updated successfully.");
                return RedirectToAction("ManageRoles");
            }

            // If update failed, log errors, add error to ModelState, and return ManageRoles view
            _logger.LogError($"Failed to update role '{role.Name}'. Errors: {string.Join(", ", result.Errors)}");
            ModelState.AddModelError("", "Failed to update the role. Please check the provided information.");
            return View("ManageRoles");
        }
        catch (Exception ex)
        {
            // If an exception occurs, log the error and return StatusCode 500
            _logger.LogError(ex, "An error occurred while updating the role.");
            return StatusCode(500, "An error occurred while updating the role.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        try
        {
            // Find the role by its ID
            var role = await _roleManager.FindByIdAsync(roleId);

            // If role not found, log a warning and return NotFound result
            if (role == null)
            {
                _logger.LogWarning($"Role with ID '{roleId}' not found.");
                return NotFound("Role not exists");
            }

            // Attempt to delete the role
            var result = await _roleManager.DeleteAsync(role);

            // If deletion succeeded, log success and redirect to ManageRoles action
            if (result.Succeeded)
            {
                _logger.LogInformation($"Role '{role.Name}' deleted successfully.");
                return RedirectToAction("ManageRoles");
            }

            // If deletion failed, log errors, add error to ModelState, and return ManageRoles view
            _logger.LogError($"Failed to delete role '{role.Name}'. Errors: {string.Join(", ", result.Errors)}");
            ModelState.AddModelError("", "Failed to delete the role. Please try again.");
            return View("ManageRoles"); // Replace "ManageRoles" with your actual view name for role management
        }
        catch (Exception ex)
        {
            // If an exception occurs, log the error and return StatusCode 500
            _logger.LogError(ex, "An error occurred while deleting the role.");
            return StatusCode(500, "An error occurred while deleting the role.");
        }
    }
    
    public IActionResult ManageRoles()
    {
        // Retrieve roles from your data source (Identity Framework using RoleManager)
        var roles = _roleManager.Roles.ToList();

        // Map roles to RoleViewModels
        var roleViewModels = roles.Select(role => new RoleViewModel
        {
            Id = role.Id,
            RoleName = role.Name
        });

        // Pass the collection of RoleViewModels to the view for rendering
        return View(roleViewModels);
    }
}