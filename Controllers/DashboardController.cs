using System.Diagnostics.CodeAnalysis;
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
    
    #region STATS METHODS

    private void MostConsultedPatientsStat()
    {
        var mostConsultedPatients = _dbContext.Patients.Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.Prescriptions.Count).Take(5).ToList();
        
        var patientsLabels = new List<string>();
        var patientsData = new List<int>();
        
        foreach (var patient in mostConsultedPatients)
        {
            patientsLabels.Add(patient.FirstName + " " + patient.LastName);
            patientsData.Add(patient.Prescriptions.Count);
        }
        
        ViewBag.PatientsLabels = patientsLabels;
        ViewBag.PatientsData = patientsData;
    }
    
    private void MostPrescribedMedicamentsStat()
    {
        var mostPrescribedMedicaments = _dbContext.Medicaments.Select(m => new
        {
            Medicament = m,
            Count = m.Prescriptions.Count
        }).OrderByDescending(m => m.Count).Take(5).Select(m => m.Medicament).ToList();
        
        var medicamentsLabels = new List<string>();
        var medicamentsData = new List<int>();
        
        foreach (var medicament in mostPrescribedMedicaments)
        {
            medicamentsLabels.Add(medicament.Name);
            medicamentsData.Add(medicament.Prescriptions.Count);
        }
        
        ViewBag.MedicamentsLabels = medicamentsLabels;
        ViewBag.MedicamentsData = medicamentsData;
    }
    
    private void MostCommonAllergiesStat()
    {
        var mostCommonAllergies = _dbContext.Allergies.Select(a => new
        {
            Allergy = a,
            Count = a.Patients.Count
        }).OrderByDescending(a => a.Count).Take(5).Select(a => a.Allergy).ToList();
        
        var allergiesLabels = new List<string>();
        var allergiesData = new List<int>();
        
        foreach (var allergy in mostCommonAllergies)
        {
            allergiesLabels.Add(allergy.Name);
            allergiesData.Add(allergy.Patients.Count);
        }
        
        ViewBag.AllergiesLabels = allergiesLabels;
        ViewBag.AllergiesData = allergiesData;
    }
    
    private void PatientsByAgeStat()
    {
        var patientsByAge = _dbContext.Patients.Where(p => p.DoctorId == UserId)
            .GroupBy(p => p.Age)
            .Select(g => new
            {
                Age = g.Key,
                Count = g.Count()
            }).ToList();
        
        var patientsByAgeLabels = new List<string>();
        var patientsByAgeData = new List<int>();
        
        for (int i = 0; i < 5; i++)
        {
            var min = i * 20;
            var max = (i + 1) * 20;
            
            var label = $"{min}-{max} ans";
            var count = patientsByAge.Where(p => p.Age >= min && p.Age < max).Sum(p => p.Count);
            
            patientsByAgeLabels.Add(label);
            patientsByAgeData.Add(count);
        }
        
        ViewBag.PatientsByAgeLabels = patientsByAgeLabels;
        ViewBag.PatientsByAgeData = patientsByAgeData;
    }
    
    #endregion
    
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
        
        MostConsultedPatientsStat();
        MostPrescribedMedicamentsStat();
        MostCommonAllergiesStat();
        PatientsByAgeStat();
        
        return View(model);
    }
}