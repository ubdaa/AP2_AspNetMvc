using MedManager.Data;
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
            .Where(m => m.Category.ToString().Contains(q) || m.Type.ToString().Contains(q) || m.Name.Contains(q)
            || m.Allergies.Any(a => model.Allergies.Contains(a)) || m.MedicalHistories.Any(mh => model.MedicalHistories.Contains(mh)))
            .ToList();
        
        model.Prescriptions = _dbContext.Prescriptions
            .Where(p => p.Medicaments.Any(m => model.Medicaments.Contains(m)))
            .ToList();
        
        return View(model);
    }
}