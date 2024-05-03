using Microsoft.AspNetCore.Mvc;

namespace Profkom.Controllers;

public class InfoController(
    
    ) : Controller
{
    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated && ( User.IsInRole("Admin") || User.IsInRole("Moderator")))
        {
            return View("TeamInfo");
        }

        if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
        {
            return View("MemberInfo");
        }
        
        if (User.Identity.IsAuthenticated && User.IsInRole("Volunteer"))
        {
            return View("VolunteerInfo");
        }

        return View("GuestInfo");
    }
}