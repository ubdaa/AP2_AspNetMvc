using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers;

[Authorize]
public class PrescriptionController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<Doctor> _userManager;
    
    public PrescriptionController(ApplicationDbContext dbContext, UserManager<Doctor> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        PatientListViewModel model = new();
        
        model.Patients = _dbContext.Patients.ToList();
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(PatientListViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Patients = _dbContext.Patients.ToList();
            return View(model);
        }
        
        var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.PatientId == model.PatientId);
        
        if (patient == null)
        {
            return NotFound();
        }
        
        var doctorId = _userManager.GetUserId(User);
        
        if (doctorId == null) return NotFound();
        
        await _dbContext.Prescriptions.AddAsync(new Prescription
        {
            PatientId = patient.PatientId,
            Patient = patient,
            DoctorId = doctorId,
            Doctor = (await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == doctorId))!,
        });
        
        await _dbContext.SaveChangesAsync();
        
        return RedirectToAction("Create");
    }
}