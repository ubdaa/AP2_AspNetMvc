using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class SearchController : Controller
{
    public IActionResult Result(string q)
    {
        return View();
    }
}