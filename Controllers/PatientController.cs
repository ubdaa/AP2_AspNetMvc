using MedManager.Data;
using MedManager.ViewModel.Patient;
using MedManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        
        return View(pvm);
    }

    [HttpPost]
    public IActionResult Add(PatientViewModel pvm)
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
        _dbContext.Patients.Add(patient);
        
        _dbContext.SaveChanges();
        
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
    public IActionResult Edit(int id)
    {
        Patient? patient =  _dbContext.Patients.FirstOrDefault(p => p.PatientId == id);

        if (patient == null) return NotFound();

        return View(patient);
    }

    /*[HttpPost]
    public IActionResult Edit(Patient patient)
    {
        Patient? patientDb = _dbContext.Patients.FirstOrDefault(p => p.PatientId == patient.PatientId);
        if (patientDb == null) return NotFound();

        patientDb.FirstName = patient.FirstName;
        patientDb.LastName = patient.LastName;
        patientDb.Age = patient.Age;
        patientDb.Gender = patient.Gender;
        patientDb.Height = patient.Height;
        patientDb.Weight = patient.Weight;
        
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
    }*/

    [HttpGet]
    public IActionResult ShowDetails(int id)
    {
        Patient? patient =  _dbContext.Patients.FirstOrDefault(p => p.PatientId == id);
        if (patient == null) return NotFound();

        return View(patient);
    }
}