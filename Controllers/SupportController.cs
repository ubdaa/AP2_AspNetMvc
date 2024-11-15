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

    [HttpPost]
    public IActionResult ContactMe()
    {
        // Send an email to the support team
        
        return RedirectToAction("Index");
    }
}