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

        patient.PatientId = _patients.Count;
        _patients.Add(patient);
        
        return RedirectToAction("Index");
    }
}