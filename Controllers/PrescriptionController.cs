using MedManager.Data;
using MedManager.Models;
using MedManager.Services;
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
    
    private string UserId => _userManager.GetUserId(User);
    
    public PrescriptionController(ApplicationDbContext dbContext, UserManager<Doctor> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(_dbContext.Prescriptions.Include(p => p.Patient).Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.PrescriptionId).ToList());
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
            ModelState.AddModelError("", "Veuillez sélectionner un patient.");
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
        
        return RedirectToAction("Edit", new { id = prescription.Entity.PrescriptionId });
    }
    
    public IActionResult Delete(int id)
    {
        var prescription = _dbContext.Prescriptions.FirstOrDefault(x => x.PrescriptionId == id);
        
        if (prescription == null)
        {
            return NotFound();
        }
        
        _dbContext.Prescriptions.Remove(prescription);
        _dbContext.SaveChanges();

        return RedirectToAction("Index");
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
        
        var model = new PrescriptionViewModel()
        {
            AdditionalInformation = prescription.AdditionalInformation,
            Dosage = prescription.Dosage,
            EndDate = prescription.EndDate,
            StartDate = prescription.StartDate,
            MedicamentsPatient = medicamentList,
            PrescriptionId = prescription.PrescriptionId,
            Patient = prescription.Patient,
            MedicamentsPrescription = prescription.Medicaments,
            IsEditing = false
        };

        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
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
        
        var model = new PrescriptionViewModel()
        {
            AdditionalInformation = prescription.AdditionalInformation,
            Dosage = prescription.Dosage,
            EndDate = prescription.EndDate,
            StartDate = prescription.StartDate,
            MedicamentsPatient = medicamentList,
            PrescriptionId = prescription.PrescriptionId,
            Patient = prescription.Patient,
            MedicamentsPrescription = prescription.Medicaments
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PrescriptionViewModel model)
    {
        if (!ModelState.IsValid || model.EndDate < model.StartDate)
        {
            var p = await _dbContext.Prescriptions
                .Include(p => p.Patient)
                .ThenInclude(patient => patient.Allergies)
                .Include(p => p.Doctor)
                .Include(p => p.Medicaments)
                .Include(prescription => prescription.Patient)
                .ThenInclude(patient => patient.MedicalHistories)
                .FirstOrDefaultAsync(p => p.PrescriptionId == model.PrescriptionId);
            
            var allergiesPatient = p.Patient.Allergies.Select(pa => pa.AllergyId).ToList();
            var medicalHistoryPatient = p.Patient.MedicalHistories.Select(mh => mh.MedicalHistoryId).ToList();
            var medicamentList = await _dbContext.Medicaments
                .Where(m => !m.Allergies.Any(a => allergiesPatient.Contains(a.AllergyId)) && !m.MedicalHistories.Any(mh => medicalHistoryPatient.Contains(mh.MedicalHistoryId)))
                .ToListAsync();

            model.Patient = p.Patient;
            model.MedicamentsPatient = medicamentList;
            model.MedicamentsPrescription = p.Medicaments;

            if (!(model.EndDate < model.StartDate)) return View(model);
            
            ModelState.AddModelError("EndDate", "La date de fin doit être supérieure à la date de début.");
            return View(model);

        }
        
        var prescription = _dbContext.Prescriptions.FirstOrDefault(p => p.PrescriptionId == model.PrescriptionId);
        if (prescription == null)
        {
            return NotFound();
        }
        
        prescription.StartDate = model.StartDate;
        prescription.EndDate = model.EndDate;
        prescription.Dosage = model.Dosage;
        prescription.AdditionalInformation = model.AdditionalInformation;
        
        _dbContext.Prescriptions.Update(prescription);
        _dbContext.SaveChanges();
        
        return RedirectToAction("Edit", new { id = prescription.PrescriptionId });
    }

    [HttpGet]
    public IActionResult AddMedicament(int id, int medicamentId)
    {
        var medicament = _dbContext.Medicaments.FirstOrDefault(m => m.MedicamentId == medicamentId);
        
        if (medicament == null)
        {
            return RedirectToAction("Edit", new { id });
        }

        try
        {
            var prescription = _dbContext.Prescriptions
                .Include(p => p.Medicaments)
                .FirstOrDefault(p => p.PrescriptionId == id);
            
            if (prescription == null)
            {
                return RedirectToAction("Edit", new { id });
            }
            
            prescription.Medicaments.Add(medicament);
            _dbContext.SaveChanges();
        }
        catch
        {
            // ignored
        }

        return RedirectToAction("Edit", new { id });
    }

    public IActionResult RemoveMedicament(int id, int medicamentId)
    {
        var prescription = _dbContext.Prescriptions
            .Include(p => p.Medicaments)
            .FirstOrDefault(p => p.PrescriptionId == id);
        
        var medicament = prescription?.Medicaments.FirstOrDefault(m => m.MedicamentId == medicamentId);
        
        if (prescription == null || medicament == null) return RedirectToAction("Edit", new { id });

        try
        {
            prescription.Medicaments.Remove(medicament);
            _dbContext.SaveChanges();
        } catch {
            // ignored
        }
        
        return RedirectToAction("Edit", new { id });
    }
    
    [HttpGet]
    public IActionResult ExportPdf(int id)
    {
        var prescription = _dbContext.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Doctor)
            .Include(p => p.Medicaments)
            .FirstOrDefault(p => p.PrescriptionId == id);
        
        if (prescription == null)
        {
            return NotFound();
        }
        
        var fileName = $"Prescription_{prescription.Patient.LastName}{prescription.Patient.FirstName}_{prescription.PrescriptionId}.pdf";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", fileName);
        PdfService.GeneratePrescriptionPdf(path, prescription);
        var pdf = System.IO.File.ReadAllBytes(path);
        
        return File(pdf, "application/pdf", path);
    }
}