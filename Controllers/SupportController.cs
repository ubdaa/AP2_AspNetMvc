using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class SupportController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}