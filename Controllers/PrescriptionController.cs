using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel;
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
    public async Task<IActionResult> Create()
    {
        PatientListViewModel model = new();
        
        var doctorId = _userManager.GetUserId(User);
        if (doctorId == null) return NotFound();
        
        model.Patients = await _dbContext.Patients.AsNoTracking().Where(p => p.DoctorId == doctorId).ToListAsync();
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(PatientListViewModel model)
    {
        var doctorId = _userManager.GetUserId(User);
        if (doctorId == null) return NotFound();
        
        if (!ModelState.IsValid)
        {
            model.Patients = _dbContext.Patients.Where(p => p.DoctorId == doctorId).ToList();
            return View(model);
        }
        
        var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.PatientId == model.PatientId);
        if (patient == null) return NotFound();
        
        var prescription = await _dbContext.Prescriptions.AddAsync(new Prescription
        {
            PatientId = patient.PatientId,
            Patient = patient,
            DoctorId = doctorId,
            Doctor = (await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == doctorId))!,
        });
        
        await _dbContext.SaveChangesAsync();
        
        return RedirectToAction("Details", new { id = prescription.Entity.PrescriptionId });
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var prescription = await _dbContext.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Doctor)
            .Include(p => p.Medicaments)
            .FirstOrDefaultAsync(p => p.PrescriptionId == id);
        
        if (prescription == null)
        {
            return NotFound();
        }
        
        return View(prescription);
    }
}