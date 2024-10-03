using AP2_AspNetMvc.Data;
using AP2_AspNetMvc.Models;
using AP2_AspNetMvc.ViewModel.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AP2_AspNetMvc.Controllers;

public class PatientController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    
    public PatientController(ApplicationDbContext dbContext)
    {   
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        //throw new Exception("Test Exception");
        
        return View(_dbContext.Patients);
    }

    [HttpGet]
    public IActionResult Add()
    {   
        return View();
    }

    /*[HttpPost]
    public IActionResult Add(Patient p)
    {
        var d = _dbContext.Users.First();
        p.DoctorId = d.DoctorId;
        p.Doctor = d;
        
        if (!ModelState.IsValid)
        {
            return View();
        }
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
    }*/

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