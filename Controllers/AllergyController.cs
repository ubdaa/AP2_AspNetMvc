using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class AllergyController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    
    public AllergyController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET
    public IActionResult Index()
    {
        return View(_dbContext.Allergies.ToList());
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Add(Allergy allergy)
    {
        if (!ModelState.IsValid)
        {
            return View(allergy);
        }
        
        _dbContext.Allergies.Add(new Allergy { Name = allergy.Name });
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
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
    
    public IActionResult Delete(int id)
    {
        var allergy = _dbContext.Allergies.FirstOrDefault(x => x.AllergyId == id);
        
        if (allergy == null)
        {
            return NotFound();
        }
        
        _dbContext.Allergies.Remove(allergy);
        _dbContext.SaveChanges();

        return RedirectToAction("Index");
    }
}