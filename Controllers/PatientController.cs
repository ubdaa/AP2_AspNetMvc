using AP2_AspNetMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace AP2_AspNetMvc.Controllers;

public class PatientController : Controller
{
    private static List<Patient> _patients = new List<Patient>()
    {
        new() { PatientId = 0, FirstName = "John", LastName = "Doe", Age = 18, Gender = Genders.Male, Height = "180", Weight = 70 },
        new() { PatientId = 1, FirstName = "Jane", LastName = "Doe", Age = 24, Gender = Genders.Female, Height = "160", Weight = 50 },
        new() { PatientId = 2, FirstName = "Alice", LastName = "Smith", Age = 20, Gender = Genders.Female, Height = "170", Weight = 60 },
    };
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(_patients);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(Patient patient)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        if (_patients.Count <= 0) patient.PatientId = 0; 
        else patient.PatientId = _patients.Last().PatientId + 1;
        _patients.Add(patient);
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Patient? patient = _patients.FirstOrDefault(p => p.PatientId == id);

        if (patient == null) return NotFound();

        return View(patient);
    } 
    
    [HttpPost]
    public IActionResult Delete(Patient patient)
    {
        Patient? patientTemp = _patients.FirstOrDefault(p => p.PatientId == patient.PatientId);

        if (patientTemp == null) return NotFound();

        _patients.Remove(patientTemp);
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Patient? patient = _patients.FirstOrDefault(p => p.PatientId == id);

        if (patient == null) return NotFound();

        return View(patient);
    }

    [HttpPost]
    public IActionResult Edit(Patient patient)
    {
        Patient? patientTemp = _patients.FirstOrDefault(p => p.PatientId == patient.PatientId);

        if (patientTemp == null) return NotFound();
        
        return RedirectToAction("Index");
    }
}