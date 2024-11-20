using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers;

[Authorize]
public class AllergyController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AllergyController> _logger;
    
    public AllergyController(ApplicationDbContext dbContext, ILogger<AllergyController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _dbContext.Allergies.ToListAsync());
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        Allergy allergy = new Allergy
        {
            Name = ""
        };
        return View(allergy);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(Allergy allergy)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(allergy);
            }

            await _dbContext.Allergies.AddAsync(new Allergy { Name = allergy.Name });
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding allergy");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var allergy = await _dbContext.Allergies
                .FirstOrDefaultAsync(x => x.AllergyId == id);

            if (allergy == null)
            {
                return NotFound();
            }

            return View(allergy);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while accessing allergy");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Allergy allergy)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(allergy);
            }
            
            var allergyToUpdate = await _dbContext.Allergies
                .FirstOrDefaultAsync(x => x.AllergyId == allergy.AllergyId);
            
            if (allergyToUpdate == null)
            {
                return NotFound();
            }
            
            allergyToUpdate.Name = allergy.Name;
            await _dbContext.SaveChangesAsync();
            
            return RedirectToAction("Index");
        } 
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating allergy");
            return RedirectToAction("Index", "Error");
        }
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var allergy = await _dbContext.Allergies
                .FirstOrDefaultAsync(x => x.AllergyId == id);

            if (allergy == null)
            {
                return NotFound();
            }

            _dbContext.Allergies.Remove(allergy);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting allergy");
            return RedirectToAction("Index", "Error");
        }
    }
}