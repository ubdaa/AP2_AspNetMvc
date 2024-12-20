using MedManager.Models;
using MedManager.ViewModel.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly UserManager<Doctor> _userManager;
    
    public AdminController(UserManager<Doctor> userManager)
    {
        _userManager = userManager;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel();
        
        model.Doctors = (await _userManager.GetUsersInRoleAsync("Docteur")).ToList();
        
        return View(model);
    }
}