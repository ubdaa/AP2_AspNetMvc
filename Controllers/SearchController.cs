using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers;

[Authorize]
public class SearchController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<SearchController> _logger;
    
    public SearchController(ApplicationDbContext dbContext, ILogger<SearchController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<IActionResult> Result(string? q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q)) return NotFound();

            q = q.ToLower();

            ResultViewModel model = new();
            
            model.Patients = await _dbContext.Patients
                .Where(p => p.FirstName.ToLower().Contains(q) || p.LastName.ToLower().Contains(q))
                .ToListAsync();
            
            model.Allergies = await _dbContext.Allergies
                .Where(a => a.Name.ToLower().Contains(q))
                .ToListAsync();
            
            model.MedicalHistories = await _dbContext.MedicalHistories
                .Where(mh => mh.Name.ToLower().Contains(q))
                .ToListAsync();

            model.Medicaments = await _dbContext.Medicaments
                .Include(m => m.Allergies)
                .Include(m => m.MedicalHistories)
                .Where(m => m.Name.ToLower().Contains(q) 
                            || m.Ingredients.ToLower().Contains(q) 
                            || m.Quantity.ToLower().Contains(q)
                            || m.Allergies.Any(a => a.Name.ToLower().Contains(q))
                            || m.MedicalHistories.Any(mh => mh.Name.ToLower().Contains(q)))
                .ToListAsync();
            
            model.Prescriptions = await _dbContext.Prescriptions
                .Include(p => p.Patient)
                .Where(p => p.Medicaments.Any(m => model.Medicaments.Contains(m)) 
                    || p.Patient.FirstName.ToLower().Contains(q) || p.Patient.LastName.ToLower().Contains(q))
                .ToListAsync();
            
            return View(model);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while searching");
            return RedirectToAction("Index", "Error");
        }
    }
}