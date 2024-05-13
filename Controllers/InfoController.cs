using Microsoft.AspNetCore.Mvc;

namespace Profkom.Controllers;

public class InfoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}