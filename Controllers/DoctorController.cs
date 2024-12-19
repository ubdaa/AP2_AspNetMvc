using MedManager.Models;
using MedManager.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

[Authorize(Roles = "Admin")]
public class DoctorController : Controller
{
    private readonly UserManager<Doctor> _userManager;

    public DoctorController(UserManager<Doctor> userManager)
    {
        _userManager = userManager;
    }

    // GET
    public IActionResult Index()
    {
        var model = _userManager.GetUsersInRoleAsync("Docteur").Result.ToList();
        return View(model);
    }

    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _userManager.DeleteAsync(_userManager.FindByIdAsync(id).Result!);
            TempData["SuccessMessage"] = "Docteur supprimé avec succès";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        var model = _userManager.FindByIdAsync(id).Result;

        if (model == null) return RedirectToAction("Index");
        
        var account = new AccountViewModel
        {
            Id = model.Id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Faculty = model.Faculty,
            Specialty = model.Specialty,
            Address = model.Specialty,
            Username = model.UserName
        };
        
        return View(account);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(AccountViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Erreur lors de la modification";
                return View(model);
            }

            if (model.Id == null || await _userManager.FindByIdAsync(model.Id) == null)
            {
                TempData["ErrorMessage"] = "Erreur lors de la modification, aucun identifiant trouvé";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id!);
            
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
            return RedirectToAction("Index", "Error");
        }
    }
}