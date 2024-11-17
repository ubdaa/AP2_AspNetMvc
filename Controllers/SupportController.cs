using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class SupportController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Cgu()
    {
        return View();
    }
}