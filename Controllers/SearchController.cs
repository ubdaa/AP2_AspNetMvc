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

        q = q.ToLower();

        ResultViewModel model = new();
        
        model.Patients = _dbContext.Patients
            .Where(p => p.FirstName.ToLower().Contains(q) || p.LastName.ToLower().Contains(q))
            .ToList();
        
        model.Allergies = _dbContext.Allergies
            .Where(a => a.Name.ToLower().Contains(q))
            .ToList();
        
        model.MedicalHistories = _dbContext.MedicalHistories
            .Where(mh => mh.Name.ToLower().Contains(q))
            .ToList();

        model.Medicaments = _dbContext.Medicaments
            .Where(m => m.Name.ToLower().Contains(q) 
                        || m.Ingredients.ToLower().Contains(q) 
                        || m.Quantity.ToLower().Contains(q)
                        || m.Allergies.Any(a => a.Name.ToLower().Contains(q))
                        || m.MedicalHistories.Any(mh => mh.Name.ToLower().Contains(q)))
            .ToList();
        
        model.Prescriptions = _dbContext.Prescriptions
            .Where(p => p.Medicaments.Any(m => model.Medicaments.Contains(m)))
            .ToList();
        
        return View(model);
    }
}