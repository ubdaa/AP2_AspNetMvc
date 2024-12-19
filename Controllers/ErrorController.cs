using System.Diagnostics;
using MedManager.Models;
using MedManager.ViewModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class ErrorController : Controller
{
    // GET
    public IActionResult Index()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var errorViewModel = new ErrorViewModel
        {
            RequestId = requestId
        };

        if (exceptionFeature != null)
        {
            ViewData["ErrorMessage"] = exceptionFeature.Error.Message;
        }

        return View(errorViewModel);
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
}