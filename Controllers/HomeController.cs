using System.Diagnostics;
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
        if (!User.Identity.IsAuthenticated) return View("UnauthorizedHome");
        
        var currentUser = await userManager.GetUserAsync(User);
        var usrId = currentUser?.Id;

        var volunteer = await volunteerService.GetByUserIdAsync(usrId);

        var model = new HomePageModel()
        {
            UserName = volunteer.FirstName + " " + volunteer.LastName,
            Email = currentUser.Email,
            PhoneNumber = currentUser.PhoneNumber,
        };
            
        if (User.Identity.IsAuthenticated && User.IsInRole("Volunteer"))
        {
            model.Role = "Volunteer";
            return View("VolunteerHome", model);
        }

        if (User.Identity.IsAuthenticated && User.IsInRole("Moderator"))
        {
            model.Role = "Moderator";
            return View("ModeratorHome", model);
        }
        
        if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
        {
            model.Role = "Admin";
            return View("AdminHome", model);
        }
        
        if (User.Identity.IsAuthenticated && User.IsInRole("Member") && User.IsInRole("ExVolunteer")  )
        {
            model.Role = "Ex Volunteer";
            return View("ExVolunteerHome", model);
        }
        
        if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
        {
            model.Role = "Member";
            return View("NewUser");
        }
        
        return View("UnauthorizedHome");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}