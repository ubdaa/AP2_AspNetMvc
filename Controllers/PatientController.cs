using AP2_AspNetMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace AP2_AspNetMvc.Controllers;

public class PatientController : Controller
{
    private static List<Patient> _patients = new();
    
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
        int patientIndex = _patients.FindIndex(p => p.PatientId == patient.PatientId);
        if (patientIndex == -1) return NotFound();
        
        _patients[patientIndex] = patient;
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ShowDetails(int id)
    {
        Patient? patient = _patients.FirstOrDefault(p => p.PatientId == id);
        if (patient == null) return NotFound();

        return View(patient);
    }
}