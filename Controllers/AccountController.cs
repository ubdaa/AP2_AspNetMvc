using MedManager.ViewModel.Account;
using MedManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            ModelState.AddModelError("", "Erreur lors de la connexion");
            return View(model);
        }
        
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(AccountViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new Doctor()
        {
            UserName = model.Username, 
            Email = model.Email, 
            FirstName = model.FirstName!,
            LastName = model.LastName!,
            Address = model.Address!,
            Faculty = model.Faculty!, 
            Specialty = model.Specialty!
        };
        var result = await _userManager.CreateAsync(user, model.Password!);
        
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        return RedirectToAction("Index", "Dashboard");
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    
    // GET
    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        
        if (user == null)
            return NotFound("Utilisateur introuvable ou non connecté");

        var model = new AccountViewModel
        {
            Username = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Password = user.PasswordHash,
            Address = user.Address,
            Faculty = user.Faculty,
            Specialty = user.Specialty
        };
        
        return View(model);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(AccountViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        
        if (user == null)
            return NotFound("Utilisateur introuvable ou non connecté");

        user.UserName = model.Username;
        user.NormalizedUserName = model.Username!.ToUpper();
        user.Email = model.Email;
        user.NormalizedEmail = model.Email!.ToUpper();
        user.FirstName = model.FirstName!;
        user.LastName = model.LastName!;
        user.Address = model.Address!;
        user.Faculty = model.Faculty!;
        user.Specialty = model.Specialty!;

        var passwordHasher = new PasswordHasher<Doctor>();
        user.PasswordHash = passwordHasher.HashPassword(user, model.Password!);

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded) return RedirectToAction("Index", "Dashboard");
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }
}