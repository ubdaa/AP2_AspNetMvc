using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

public class MedicalHistoryController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    
    public MedicalHistoryController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET
    public IActionResult Index()
    {
        return View(_dbContext.MedicalHistories.ToList());
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Add(MedicalHistory medicalHistory)
    {
        if (!ModelState.IsValid)
        {
            return View(medicalHistory);
        }
        
        _dbContext.MedicalHistories.Add(new MedicalHistory { Name = medicalHistory.Name });
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var medicalHistory = _dbContext.MedicalHistories.FirstOrDefault(x => x.MedicalHistoryId == id);
        
        if (medicalHistory == null)
        {
            return NotFound();
        }
        
        return View(medicalHistory);
    }
    
    [HttpPost]
    public IActionResult Edit(MedicalHistory medicalHistory)
    {
        if (!ModelState.IsValid)
        {
            return View(medicalHistory);
        }
        
        var medicalHistoryToUpdate = _dbContext.MedicalHistories.FirstOrDefault(x => x.MedicalHistoryId == medicalHistory.MedicalHistoryId);
        
        if (medicalHistoryToUpdate == null)
        {
            return NotFound();
        }
        
        medicalHistoryToUpdate.Name = medicalHistory.Name;
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
    }
    
    public IActionResult Delete(int id)
    {
        var medicalHistory = _dbContext.MedicalHistories.FirstOrDefault(x => x.MedicalHistoryId == id);
        
        if (medicalHistory == null)
        {
            return NotFound();
        }
        
        _dbContext.MedicalHistories.Remove(medicalHistory);
        _dbContext.SaveChanges();

        return RedirectToAction("Index");
    }
}