using System.Xml.Linq;
using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel.Medicament;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers;

public class MedicamentController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    
    public MedicamentController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET
    public IActionResult Index()
    {
        return View(_dbContext.Medicaments.ToList());
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        MedicamentViewModel model = new();
        
        model.MedicalHistories = _dbContext.MedicalHistories.ToList();
        model.Allergies = _dbContext.Allergies.ToList();
        
        return View(model);
    }
    
    [HttpPost]
    public IActionResult Add(MedicamentViewModel medicamentViewModel)
    {
        if (!ModelState.IsValid)
        {
            medicamentViewModel.MedicalHistories = _dbContext.MedicalHistories.ToList();
            medicamentViewModel.Allergies = _dbContext.Allergies.ToList();
            
            return View(medicamentViewModel);
        }
        
        Medicament medicament = new()
        {
            Name = medicamentViewModel.Name,
            Quantity = medicamentViewModel.Quantity,
            Ingredients = medicamentViewModel.Ingredients
        };
        
        foreach (var allergyId in medicamentViewModel.SelectedAllergyIds)
        {
            var allergy = _dbContext.Allergies.FirstOrDefault(x => x.AllergyId == allergyId);
            
            if (allergy != null)
            {
                medicament.Allergies.Add(allergy);
            }
        }
        
        foreach (var medicalHistoryId in medicamentViewModel.SelectedMedicalHistoryIds)
        {
            var medicalHistory = _dbContext.MedicalHistories.FirstOrDefault(x => x.MedicalHistoryId == medicalHistoryId);
            
            if (medicalHistory != null)
            {
                medicament.MedicalHistories.Add(medicalHistory);
            }
        }
        
        _dbContext.Medicaments.Add(medicament);
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var medicament = await _dbContext.Medicaments
            .Include(m => m.Allergies)
            .Include(m => m.MedicalHistories)
            .FirstOrDefaultAsync(x => x.MedicamentId == id);
        
        if (medicament == null)
        {
            return NotFound();
        }
        
        MedicamentViewModel model = new()
        {
            MedicamentId = medicament.MedicamentId,
            Name = medicament.Name,
            Quantity = medicament.Quantity,
            Ingredients = medicament.Ingredients,
            MedicalHistories = _dbContext.MedicalHistories.ToList(),
            Allergies = _dbContext.Allergies.ToList(),
            SelectedAllergyIds = medicament.Allergies.Select(a => a.AllergyId).ToList(),
            SelectedMedicalHistoryIds = medicament.MedicalHistories.Select(m => m.MedicalHistoryId).ToList()
        };
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(MedicamentViewModel medicamentViewModel)
    {
        if (!ModelState.IsValid)
        {
            medicamentViewModel.MedicalHistories = _dbContext.MedicalHistories.ToList();
            medicamentViewModel.Allergies = _dbContext.Allergies.ToList();
            
            return View(medicamentViewModel);
        }
        
        Medicament? medicamentToUpdate = await _dbContext.Medicaments
            .Include(m => m.Allergies)
            .Include(m => m.MedicalHistories)
            .FirstOrDefaultAsync(x => x.MedicamentId == medicamentViewModel.MedicamentId);
        
        if (medicamentToUpdate == null)
        {
            return NotFound();
        }
        
        medicamentToUpdate.Name = medicamentViewModel.Name;
        medicamentToUpdate.Quantity = medicamentViewModel.Quantity;
        medicamentToUpdate.Ingredients = medicamentViewModel.Ingredients;
        
        medicamentToUpdate.Allergies.Clear();
        
        var selectedAllergies = await _dbContext.Allergies
            .Where(a => medicamentViewModel.SelectedAllergyIds.Contains(a.AllergyId))
            .ToListAsync();
        foreach (var allergy in selectedAllergies)
        {
            medicamentToUpdate.Allergies.Add(allergy);
        }
        
        medicamentToUpdate.MedicalHistories.Clear();
        
        var selectedMedicalHistories = await _dbContext.MedicalHistories
            .Where(a => medicamentViewModel.SelectedMedicalHistoryIds.Contains(a.MedicalHistoryId))
            .ToListAsync();
        foreach (var medicalHistory in selectedMedicalHistories)
        {
            medicamentToUpdate.MedicalHistories.Add(medicalHistory);
        }

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!_dbContext.Medicaments.Any(x => x.MedicamentId == medicamentViewModel.MedicamentId))
            {
                return NotFound();
            }
        }
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var medicament = await _dbContext.Medicaments.FirstOrDefaultAsync(x => x.MedicamentId == id);
        
        if (medicament == null)
        {
            return NotFound();
        }
        
        _dbContext.Medicaments.Remove(medicament);
        await _dbContext.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var medicament = await _dbContext.Medicaments
            .Include(m => m.Allergies)
            .Include(m => m.MedicalHistories)
            .FirstOrDefaultAsync(x => x.MedicamentId == id);
        
        if (medicament == null)
        {
            return NotFound();
        }
        
        return View(medicament);
    }
}