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
    
    private string UserId => _userManager.GetUserId(User);
    
    public PatientController(ApplicationDbContext dbContext, UserManager<Doctor> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(_dbContext.Patients.Where(p => p.DoctorId == UserId).ToList());
    }

    [HttpGet]
    public IActionResult Add()
    {
        PatientViewModel pvm = new PatientViewModel();

        pvm.Allergies = _dbContext.Allergies.ToList();
        pvm.MedicalHistories = _dbContext.MedicalHistories.ToList();
        pvm.DrpAllergies = _dbContext.Allergies
            .Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToList();
        pvm.DrpMedicalHistories = _dbContext.MedicalHistories
            .Select(x => new SelectListItem { Text = x.Name, Value = x.MedicalHistoryId.ToString() }).ToList();
        
        return View(pvm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(PatientViewModel pvm)
    {
        if (!ModelState.IsValid)
        {
            pvm.Allergies = _dbContext.Allergies.ToList();
            pvm.MedicalHistories = _dbContext.MedicalHistories.ToList();
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
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Patient? patient = _dbContext.Patients.FirstOrDefault(p => p.PatientId == id);

        if (patient == null) return NotFound();

        return View(patient);
    } 
    
    /*[HttpPost]
    public IActionResult Delete(Patient patient)
    {
        Patient? patientTemp =  _dbContext.Patients.FirstOrDefault(p => p.PatientId == patient.PatientId);

        if (patientTemp == null) return NotFound();

        _dbContext.Patients.Remove(patientTemp);
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
    }*/

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Patient? patient =  await _dbContext.Patients
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
            DrpAllergies = _dbContext.Allergies.Select(x => new SelectListItem { Text = x.Name, Value = x.AllergyId.ToString() }).ToList(),
            DrpMedicalHistories = _dbContext.MedicalHistories.Select(x => new SelectListItem { Text = x.Name, Value = x.MedicalHistoryId.ToString() }).ToList(),
        };
        
        return View(pvm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PatientViewModel pvm)
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

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!_dbContext.Patients.Any(p => p.PatientId == pvm.PatientId))
            {
                return NotFound();
            }
        }
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ShowDetails(int id)
    {
        Patient? patient =  _dbContext.Patients.FirstOrDefault(p => p.PatientId == id);
        if (patient == null) return NotFound();

        return View(patient);
    }
    
    [HttpGet]
    public async Task<IActionResult> CreatePrescription(int id)
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

        return RedirectToAction("Details", "Prescription", new { id = prescription.Entity.PrescriptionId });
    }
}