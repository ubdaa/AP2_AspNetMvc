using MedManager.ViewModel.Account;
using MedManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MedManager.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<Doctor> _signInManager;
    private readonly UserManager<Doctor> _userManager;
    
    public AccountController(SignInManager<Doctor> signInManager, UserManager<Doctor> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Erreur lors de la connexion");
            return View(model);
        }
        
        return RedirectToAction("Index", "Patient");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new Doctor() { UserName = model.Username, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
        var result = await _userManager.CreateAsync(user, model.Password!);
        
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        return RedirectToAction("Index", "Patient");
    }
    
    // GET
    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }
}