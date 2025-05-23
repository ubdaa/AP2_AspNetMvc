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
    private readonly ILogger<AccountController> _logger;
    
    public AccountController(SignInManager<Doctor> signInManager, UserManager<Doctor> userManager, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager
                .PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Erreur lors de la connexion");
                TempData["ErrorMessage"] = "Erreur lors de la connexion";
                return View(model);
            }
            
            TempData["SuccessMessage"] = "Bienvenue sur votre tableau de bord !";

            return RedirectToAction("Index", User.IsInRole("Admin") ? "Admin" : "Dashboard");
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while logging in");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(AccountViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Erreur lors de l'inscription";
                return View(model);
            }

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
                
                TempData["ErrorMessage"] = "Erreur lors de l'inscription";
                return View(model);
            }
            
            string role = model.Role.ToString();
            
            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction("Index", "Dashboard");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while registering");
            return RedirectToAction("Index", "Error");
        }
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
            Specialty = user.Specialty,
        };
        
        return View(model);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(AccountViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Erreur lors de la modification";
                return View(model);
            }

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

            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordHasher = new PasswordHasher<Doctor>();
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password!);
            }

            var result = await _userManager.UpdateAsync(user);
            
            var role = model.Role.ToString();
            
            await _signInManager.SignOutAsync();
            await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(user).Result);
            await _userManager.AddToRoleAsync(user, role);
            await _signInManager.SignInAsync(user, true);

            if (result.Succeeded) return RedirectToAction("Index", "Dashboard");
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                TempData["ErrorMessage"] = "Erreur lors de la modification";
            }
            
            TempData["SuccessMessage"] = "Votre profil a été mis à jour avec succès";
            return View(model);
        } 
        catch (Exception e)
        {
            _logger.LogError(e, "Error while editing user");
            return RedirectToAction("Index", "Error");
        }
    }
    
    public IActionResult EditRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(RoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Erreur lors de la modification";
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        
        if (user == null)
            return NotFound("Utilisateur introuvable ou non connecté");
        
        var role = model.Role.ToString();

        await _signInManager.SignOutAsync();
        
        await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(user).Result);
        await _userManager.AddToRoleAsync(user, role);

        await _signInManager.SignInAsync(user, true);
        
        TempData["SuccessMessage"] = "Votre rôle a été mis à jour avec succès";
        
        
        return RedirectToAction("Index", "Dashboard");
    }
}