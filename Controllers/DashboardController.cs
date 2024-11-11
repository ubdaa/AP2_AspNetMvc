using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}