using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel.Search;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class SearchController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    
    public SearchController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IActionResult Result(string? q)
    {
        if (string.IsNullOrWhiteSpace(q)) return NotFound();

        ResultViewModel model = new();
        
        model.Patients = _dbContext.Patients
            .Where(p => p.FirstName.Contains(q) || p.LastName.Contains(q))
            .ToList();
        
        model.Allergies = _dbContext.Allergies
            .Where(a => a.Name.Contains(q))
            .ToList();
        
        model.MedicalHistories = _dbContext.MedicalHistories
            .Where(mh => mh.Name.Contains(q))
            .ToList();

        model.Medicaments = _dbContext.Medicaments
            .Where(m => m.Name.Contains(q) || m.Ingredients.Contains(q) || m.Quantity.Contains(q)
                        || m.Allergies.Any(a => a.Name.Contains(q))
                        || m.MedicalHistories.Any(mh => mh.Name.Contains(q)))
            .ToList();
        
        model.Prescriptions = _dbContext.Prescriptions
            .Where(p => p.Medicaments.Any(m => model.Medicaments.Contains(m)))
            .ToList();
        
        return View(model);
    }
}