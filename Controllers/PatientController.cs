using MedManager.Data;
using MedManager.ViewModel.Patient;
using MedManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Controllers;

[Authorize]
public class PatientController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<Doctor> _userManager;
    private readonly ILogger<PatientController> _logger;

    private string UserId => _userManager.GetUserId(User);

    public PatientController(ApplicationDbContext dbContext, UserManager<Doctor> userManager,
        ILogger<PatientController> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _dbContext.Patients.Where(p => p.DoctorId == UserId).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        try
        {
            var pvm = new PatientViewModel
            {
                Allergies = await _dbContext.Allergies.ToListAsync(),
                MedicalHistories = await _dbContext.MedicalHistories.ToListAsync(),
                DrpAllergies = await _dbContext.Allergies
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToListAsync(),
                DrpMedicalHistories = await _dbContext.MedicalHistories
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.MedicalHistoryId.ToString() })
                    .ToListAsync()
            };

            return View(pvm);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding patient");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(PatientViewModel pvm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                pvm.Allergies = await _dbContext.Allergies.ToListAsync();
                pvm.MedicalHistories = await _dbContext.MedicalHistories.ToListAsync();
                return View(pvm);
            }

            Patient patient = new Patient
            {
                FirstName = pvm.FirstName,
                LastName = pvm.LastName,
                BirthDate = pvm.BirthDate,
                Address = pvm.Address,
                Age = (int)(DateTime.Now - pvm.BirthDate.ToDateTime(new TimeOnly(0, 0, 0))).TotalDays / 365,
                Gender = pvm.Gender,
                Height = pvm.Height,
                Weight = pvm.Weight,
                SocialSecurityNumber = pvm.SocialSecurityNumber,
                DoctorId = UserId
            };

            patient.Allergies.Clear();

            var selectedAllergies = await _dbContext.Allergies
                .Where(a => pvm.SelectedAllergyIds.Contains(a.AllergyId))
                .ToListAsync();
            foreach (var allergy in selectedAllergies)
            {
                patient.Allergies.Add(allergy);
            }

            patient.MedicalHistories.Clear();

            var selectedMedicalHistories = await _dbContext.MedicalHistories
                .Where(a => pvm.SelectedMedicalHistoryIds.Contains(a.MedicalHistoryId))
                .ToListAsync();
            foreach (var medicalHistory in selectedMedicalHistories)
            {
                patient.MedicalHistories.Add(medicalHistory);
            }

            _dbContext.Patients.Add(patient);

            await _dbContext.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Votre patient a été ajouté avec succès";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding patient");
            return RedirectToAction("Index", "Error");
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var patientToDelete = await _dbContext.Patients
                .Include(p => p.Prescriptions).Where(p => p.PatientId == id)
                .FirstOrDefaultAsync();
            if (patientToDelete == null) return NotFound();

            _dbContext.Patients.Remove(patientToDelete);
            await _dbContext.SaveChangesAsync();
            TempData["ErrorMessage"] = "Votre patient a été supprimé avec succès";
            return RedirectToAction("Index", "Patient");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting patient");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            Patient? patient = await _dbContext.Patients
                .Include(p => p.Allergies)
                .Include(p => p.MedicalHistories)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null) return NotFound();

            PatientViewModel pvm = new PatientViewModel
            {
                PatientId = id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                Height = patient.Height,
                Weight = patient.Weight,
                Address = patient.Address,
                SocialSecurityNumber = patient.SocialSecurityNumber,
                Allergies = await _dbContext.Allergies.ToListAsync(),
                MedicalHistories = await _dbContext.MedicalHistories.ToListAsync(),
                SelectedAllergyIds = patient.Allergies.Select(a => a.AllergyId).ToList(),
                SelectedMedicalHistoryIds = patient.MedicalHistories.Select(m => m.MedicalHistoryId).ToList(),
                DrpAllergies = await _dbContext.Allergies
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToListAsync(),
                DrpMedicalHistories = await _dbContext.MedicalHistories.Select(x => new SelectListItem
                    { Text = x.Name, Value = x.MedicalHistoryId.ToString() }).ToListAsync(),
            };

            return View(pvm);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while editing patient");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PatientViewModel pvm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                pvm.Allergies = _dbContext.Allergies.ToList();
                pvm.MedicalHistories = _dbContext.MedicalHistories.ToList();
                return View(pvm);
            }

            Patient? patient = await _dbContext.Patients
                .Include(p => p.Allergies)
                .Include(p => p.MedicalHistories)
                .FirstOrDefaultAsync(p => p.PatientId == pvm.PatientId);

            if (patient == null) return NotFound();

            patient.FirstName = pvm.FirstName;
            patient.LastName = pvm.LastName;
            patient.BirthDate = pvm.BirthDate;
            patient.Address = pvm.Address;
            patient.Age = (int)(DateTime.Now - pvm.BirthDate.ToDateTime(new TimeOnly(0, 0, 0))).TotalDays / 365;
            patient.Gender = pvm.Gender;
            patient.Height = pvm.Height;
            patient.Weight = pvm.Weight;
            patient.SocialSecurityNumber = pvm.SocialSecurityNumber;

            patient.Allergies.Clear();

            var selectedAllergies = await _dbContext.Allergies
                .Where(a => pvm.SelectedAllergyIds.Contains(a.AllergyId))
                .ToListAsync();
            foreach (var allergy in selectedAllergies)
            {
                patient.Allergies.Add(allergy);
            }

            patient.MedicalHistories.Clear();

            var selectedMedicalHistories = await _dbContext.MedicalHistories
                .Where(a => pvm.SelectedMedicalHistoryIds.Contains(a.MedicalHistoryId))
                .ToListAsync();
            foreach (var medicalHistory in selectedMedicalHistories)
            {
                patient.MedicalHistories.Add(medicalHistory);
            }

            _dbContext.Entry(patient).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Votre patient a été modifié avec succès";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while editing patient");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.PatientId == id);
            if (patient == null) return NotFound();

            return View(patient);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while showing patient details");
            return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreatePrescription(int id)
    {
        try
        {
            var patient = await _dbContext.Patients.FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            var doctorId = _userManager.GetUserId(User);

            if (doctorId == null) return NotFound();

            var prescription = await _dbContext.Prescriptions.AddAsync(new Prescription
            {
                PatientId = patient.PatientId,
                Patient = patient,
                DoctorId = doctorId,
                Doctor = (await _dbContext.Users.FirstOrDefaultAsync(d => d.Id == doctorId))!,
            });

            await _dbContext.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"L'ordonnance de {patient.FirstName} {patient.LastName} a été créée avec succès";
            return RedirectToAction("Edit", "Prescription", new { id = prescription.Entity.PrescriptionId });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating prescription");
            return RedirectToAction("Index", "Error");
        }
    } 
}