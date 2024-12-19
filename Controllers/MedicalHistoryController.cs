using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers;

[Authorize(Roles = "Docteur")]
public class MedicalHistoryController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<MedicalHistory> _logger;
    
    public MedicalHistoryController(ApplicationDbContext dbContext, ILogger<MedicalHistory> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _dbContext.MedicalHistories.ToListAsync());
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        MedicalHistory medicalHistory = new MedicalHistory
        {
            Name = ""
        };
        return View(medicalHistory);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(MedicalHistory medicalHistory)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(medicalHistory);
            }

            await _dbContext.MedicalHistories.AddAsync(new MedicalHistory { Name = medicalHistory.Name });
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Votre antécédent médical a été ajouté avec succès";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding medical history");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var medicalHistory = await _dbContext.MedicalHistories
                .FirstOrDefaultAsync(x => x.MedicalHistoryId == id);

            if (medicalHistory == null)
            {
                return NotFound();
            }

            return View(medicalHistory);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while editing medical history");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(MedicalHistory medicalHistory)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(medicalHistory);
            }

            var medicalHistoryToUpdate = await _dbContext.MedicalHistories
                    .FirstOrDefaultAsync(x => x.MedicalHistoryId == medicalHistory.MedicalHistoryId);

            if (medicalHistoryToUpdate == null)
            {
                return NotFound();
            }

            medicalHistoryToUpdate.Name = medicalHistory.Name;
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Votre antécédent médical a été modifié avec succès";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating medical history");
            return RedirectToAction("Index", "Error");
        }
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var medicalHistory = await _dbContext.MedicalHistories
                .FirstOrDefaultAsync(x => x.MedicalHistoryId == id);

            if (medicalHistory == null)
            {
                return NotFound();
            }

            _dbContext.MedicalHistories.Remove(medicalHistory);
            await _dbContext.SaveChangesAsync();

            TempData["ErrorMessage"] = "Votre antécédent médical a été supprimé avec succès";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting medical history");
            return RedirectToAction("Index", "Error");
        }
    }
}