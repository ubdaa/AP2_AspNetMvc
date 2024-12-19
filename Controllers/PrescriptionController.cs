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

[Authorize(Roles = "Docteur")]
public class PrescriptionController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<Doctor> _userManager;
    private readonly ILogger<PrescriptionController> _logger;
    
    private string UserId => _userManager.GetUserId(User);

    public PrescriptionController(ApplicationDbContext dbContext, UserManager<Doctor> userManager, ILogger<PrescriptionController> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _dbContext.Prescriptions
            .Include(p => p.Patient)
            .Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.PrescriptionId)
            .ToListAsync());
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            PatientListViewModel model = new();

            var doctorId = _userManager.GetUserId(User);
            if (doctorId == null) return NotFound();

            model.Patients = await _dbContext.Patients.AsNoTracking().Where(p => p.DoctorId == doctorId).ToListAsync();

            return View(model);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating prescription");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(PatientListViewModel model)
    {
        try
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
            
            TempData["SuccessMessage"] = $"L'ordonnance de {patient.FirstName} {patient.LastName} a été créée avec succès";
            return RedirectToAction("Edit", new { id = prescription.Entity.PrescriptionId });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating prescription");
            return RedirectToAction("Index", "Error");
        }
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var prescription = await _dbContext.Prescriptions.FirstOrDefaultAsync(x => x.PrescriptionId == id);

            if (prescription == null)
            {
                return NotFound();
            }

            _dbContext.Prescriptions.Remove(prescription);
            await _dbContext.SaveChangesAsync();

            TempData["ErrorMessage"] = "Votre ordonnance a été supprimée avec succès";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting prescription");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
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
            var medicalHistoryPatient =
                prescription.Patient.MedicalHistories.Select(mh => mh.MedicalHistoryId).ToList();
            var medicamentList = await _dbContext.Medicaments
                .Where(m => !m.Allergies.Any(a => allergiesPatient.Contains(a.AllergyId)) &&
                            !m.MedicalHistories.Any(mh => medicalHistoryPatient.Contains(mh.MedicalHistoryId)))
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
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting prescription details");
            return RedirectToAction("Index", "Error");
        } 
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(e, "Error while editing prescription");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PrescriptionViewModel model)
    {
        try
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
            await _dbContext.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Votre ordonnance a été modifiée avec succès";
            return RedirectToAction("Edit", new { id = prescription.PrescriptionId });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while editing prescription");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddMedicament(int id, int medicamentId)
    {
        try
        {
            var medicament = await _dbContext.Medicaments.FirstOrDefaultAsync(m => m.MedicamentId == medicamentId);
            
            if (medicament == null)
            {
                return RedirectToAction("Edit", new { id });
            }

            var prescription = await _dbContext.Prescriptions
                .Include(p => p.Medicaments)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);
            
            if (prescription == null)
            {
                return RedirectToAction("Edit", new { id });
            }
            
            prescription.Medicaments.Add(medicament);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Le médicament {medicament.Name} a été ajouté à l'ordonnance avec succès";
            return RedirectToAction("Edit", new { id });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding medicament to prescription");
            return RedirectToAction("Index", "Error");
        }
    }

    public async Task<IActionResult> RemoveMedicament(int id, int medicamentId)
    {
        try
        {
            var prescription = await _dbContext.Prescriptions
                .Include(p => p.Medicaments)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);
            
            var medicament = prescription?.Medicaments.FirstOrDefault(m => m.MedicamentId == medicamentId);
            
            if (prescription == null || medicament == null) return RedirectToAction("Edit", new { id });

            prescription.Medicaments.Remove(medicament);
            await _dbContext.SaveChangesAsync();
            
            TempData["ErrorMessage"] = $"Le médicament {medicament.Name} a été retiré de l'ordonnance avec succès";
            return RedirectToAction("Edit", new { id });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while removing medicament from prescription");
            return RedirectToAction("Index", "Error");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> ExportPdf(int id)
    {
        try
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
            
            var fileName = $"Prescription_{prescription.Patient.LastName}{prescription.Patient.FirstName}_{prescription.PrescriptionId}.pdf";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", fileName);
            PdfService.GeneratePrescriptionPdf(path, prescription);
            var pdf = await System.IO.File.ReadAllBytesAsync(path);
            
            return File(pdf, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while exporting prescription to pdf");
            return RedirectToAction("Index", "Error");
        }
    }
}