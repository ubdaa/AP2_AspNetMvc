using MedManager.Data;
using MedManager.Models;
using MedManager.ViewModel.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedManager.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<Doctor> _userManager;
    
    private string UserId => _userManager.GetUserId(User);
    
    public DashboardController(ApplicationDbContext dbContext, UserManager<Doctor> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        DashboardViewModel model = new DashboardViewModel();

        // données principales pour le tableau de bord
        model.Patients = _dbContext.Patients.Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.PatientId).Take(10).ToList();
        model.Prescriptions = _dbContext.Prescriptions.Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.PrescriptionId).Take(10).ToList();
        
        model.TotalPatients = _dbContext.Patients.Count(p => p.DoctorId == UserId);
        model.TotalPrescriptions = _dbContext.Prescriptions.Count(p => p.DoctorId == UserId);
        
        // données pour les statistiques
        List<Patient> MostConsultedPatients = _dbContext.Patients.Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.Prescriptions.Count).Take(5).ToList();
        
        var PatientsLabels = new List<string>();
        var PatientsData = new List<int>();
        
        foreach (var patient in MostConsultedPatients)
        {
            PatientsLabels.Add(patient.FirstName + " " + patient.LastName);
            PatientsData.Add(patient.Prescriptions.Count);
        }
        
        ViewBag.PatientsLabels = PatientsLabels;
        ViewBag.PatientsData = PatientsData;
        
        return View(model);
    }
}