using System.Xml.Linq;
using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel.Medicament;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers;

[Authorize]
public class MedicamentController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AllergyController> _logger;
    
    public MedicamentController(ApplicationDbContext dbContext, ILogger<AllergyController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _dbContext.Medicaments.ToListAsync());
    }
    
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        try
        {
            MedicamentViewModel model = new()
            {
                DrpAllergies = await _dbContext.Allergies
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToListAsync(),
                DrpMedicalHistories = await _dbContext.MedicalHistories
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.MedicalHistoryId.ToString() })
                    .ToListAsync()
            };

            return View(model);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding medicament");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(MedicamentViewModel medicamentViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                medicamentViewModel.DrpAllergies = await _dbContext.Allergies
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToListAsync();
                medicamentViewModel.DrpMedicalHistories = await _dbContext.MedicalHistories
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.MedicalHistoryId.ToString() })
                    .ToListAsync();

                return View(medicamentViewModel);
            }

            Medicament medicament = new()
            {
                Name = medicamentViewModel.Name,
                Quantity = medicamentViewModel.Quantity,
                Ingredients = medicamentViewModel.Ingredients,
                Type = medicamentViewModel.MedicamentType,
                Category = medicamentViewModel.MedicamentCategory
            };

            foreach (var allergyId in medicamentViewModel.SelectedAllergyIds)
            {
                var allergy = await _dbContext.Allergies
                    .FirstOrDefaultAsync(x => x.AllergyId == allergyId);

                if (allergy != null)
                {
                    medicament.Allergies.Add(allergy);
                }
            }

            foreach (var medicalHistoryId in medicamentViewModel.SelectedMedicalHistoryIds)
            {
                var medicalHistory = await _dbContext.MedicalHistories
                    .FirstOrDefaultAsync(x => x.MedicalHistoryId == medicalHistoryId);

                if (medicalHistory != null)
                {
                    medicament.MedicalHistories.Add(medicalHistory);
                }
            }

            await _dbContext.Medicaments.AddAsync(medicament);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding medicament");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
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
                SelectedAllergyIds = medicament.Allergies.Select(a => a.AllergyId).ToList(),
                SelectedMedicalHistoryIds = medicament.MedicalHistories.Select(m => m.MedicalHistoryId).ToList(),
                DrpAllergies = await _dbContext.Allergies
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToListAsync(),
                DrpMedicalHistories =  await _dbContext.MedicalHistories
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.MedicalHistoryId.ToString() }).ToListAsync(),
            };
            
            return View(model);
        } 
        catch (Exception e)
        {
            _logger.LogError(e, "Error while accessing medicament");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(MedicamentViewModel medicamentViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                medicamentViewModel.DrpAllergies = await _dbContext.Allergies
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToListAsync();
                medicamentViewModel.DrpMedicalHistories = await _dbContext.MedicalHistories
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.MedicalHistoryId.ToString() }).ToListAsync();

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
            medicamentToUpdate.Type = medicamentViewModel.MedicamentType;
            medicamentToUpdate.Category = medicamentViewModel.MedicamentCategory;

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

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while editing medicament");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting medicament");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(e, "Error while accessing medicament");
            return RedirectToAction("Index", "Error");
        }
    }
}