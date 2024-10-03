using Microsoft.AspNetCore.Mvc;

namespace AP2_AspNetMvc.Controllers;

public class ErrorController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}