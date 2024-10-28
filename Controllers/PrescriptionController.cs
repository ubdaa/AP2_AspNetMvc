using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel;
using MedManager.ViewModel.Prescription;
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
            .ThenInclude(patient => patient.Allergies)
            .Include(p => p.Doctor)
            .Include(p => p.Medicaments)
            .Include(prescription => prescription.Patient)
            .ThenInclude(patient => patient.MedicalHistories)
            .FirstOrDefaultAsync(p => p.PrescriptionId == id);
        
        if (prescription == null)
        {
            return NotFound();
        }
        
        var allergiesPatient = prescription.Patient.Allergies.Select(pa => pa.AllergyId).ToList();
        var medicalHistoryPatient = prescription.Patient.MedicalHistories.Select(mh => mh.MedicalHistoryId).ToList();
        var medicamentList = await _dbContext.Medicaments
            .Where(m => !m.Allergies.Any(a => allergiesPatient.Contains(a.AllergyId)) && !m.MedicalHistories.Any(mh => medicalHistoryPatient.Contains(mh.MedicalHistoryId)))
            .ToListAsync();
        
        var model = new Tuple<Prescription, IEnumerable<Medicament>>(prescription, medicamentList);
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(FormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", new { id = model.PrescriptionId });
        }
        
        var prescription = _dbContext.Prescriptions.FirstOrDefault(p => p.PrescriptionId == model.PrescriptionId);
        if (prescription == null)
        {
            return NotFound();
        }
        
        if (model.EndDate < model.StartDate)
        {
            ModelState.AddModelError("EndDate", "La date de fin doit être supérieure à la date de début.");
            return RedirectToAction("Details", new { id = model.PrescriptionId });
        }
        
        prescription.StartDate = model.StartDate;
        prescription.EndDate = model.EndDate;
        prescription.Dosage = model.Dosage;
        prescription.AdditionalInformation = model.AdditionalInformation;
        
        _dbContext.Prescriptions.Update(prescription);
        _dbContext.SaveChanges();
        
        return RedirectToAction("Details", new { id = prescription.PrescriptionId });
    }
}