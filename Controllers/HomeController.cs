using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profkom.Data;
using Profkom.Models;
using Profkom.Services;

namespace Profkom.Controllers;

public class HomeController(
    VolunteerService volunteerService,
    UserManager<AppUser> userManager
) : Controller
{
    public async Task<IActionResult> Index()
    {
        var currentUser = await userManager.GetUserAsync(User);
        var usrId = currentUser?.Id;

        if (usrId == null) return View();

        var volunteer = await volunteerService.GetByUserIdAsync(usrId);
        
        if (volunteer == null) return View();
        
        var model = new HomePageModel
        {
            UserName = volunteer.FirstName + " " + volunteer.LastName,
            Email = currentUser.Email,
            PhoneNumber = currentUser.PhoneNumber
        };

        if (User.Identity.IsAuthenticated && User.IsInRole("Volunteer"))
        {
            model.Role = "Volunteer";
        }

        if (User.Identity.IsAuthenticated && User.IsInRole("Moderator"))
        {
            model.Role = "Moderator";
        }

        if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
        {
            model.Role = "Admin";
        }

        if (User.Identity.IsAuthenticated && User.IsInRole("Member") && User.IsInRole("ExVolunteer"))
        {
            model.Role = "Ex Volunteer";
        }

        if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
        {
            model.Role = "Member";
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}