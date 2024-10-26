using MedManager.Data;
using MedManager.ViewModel.Medicament;
using Microsoft.AspNetCore.Mvc;

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
}