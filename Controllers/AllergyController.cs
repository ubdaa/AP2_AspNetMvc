using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Index()
    {
        return View(_dbContext.Allergies.ToList());
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
    public IActionResult Add(Allergy allergy)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(allergy);
            }

            _dbContext.Allergies.Add(new Allergy { Name = allergy.Name });
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding allergy");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var allergy = _dbContext.Allergies.FirstOrDefault(x => x.AllergyId == id);
        
        if (allergy == null)
        {
            return NotFound();
        }
        
        return View(allergy);
    }
    
    [HttpPost]
    public IActionResult Edit(Allergy allergy)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(allergy);
            }
            
            var allergyToUpdate = _dbContext.Allergies.FirstOrDefault(x => x.AllergyId == allergy.AllergyId);
            
            if (allergyToUpdate == null)
            {
                return NotFound();
            }
            
            allergyToUpdate.Name = allergy.Name;
            _dbContext.SaveChanges();
            
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
            var allergy = _dbContext.Allergies.FirstOrDefault(x => x.AllergyId == id);

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