using MedManager.Data;
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
        return View();
    }
}