using MedManager.ViewModel.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        DashboardViewModel model = new DashboardViewModel();
        return View(model);
    }
}