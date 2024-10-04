using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class ErrorController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}