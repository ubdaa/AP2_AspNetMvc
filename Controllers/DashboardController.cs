using System.Diagnostics.CodeAnalysis;
using MedManager.Data;
using MedManager.Models;
using MedManager.Utils;
using MedManager.ViewModel.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    private ChartScriptViewModel MostConsultedPatientsStat()
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

        return new ChartScriptViewModel()
        {
            Labels = patientsLabels,
            Data = patientsData,
            Label = "Consultations",
            ChartName = "patientsChart",
            ChartType = "pie",
            BorderColor = new[]
            {
                "rgba(54, 162, 235, 1)",
                "rgba(255, 99, 132, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)"
            },
            BackgroundColor = new[]
            {
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 99, 132, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)"
            }
        };
    }
    
    private ChartScriptViewModel MostPrescribedMedicamentsStat()
    {
        var mostPrescribedMedicaments = _dbContext.Medicaments
            .Include(m => m.Prescriptions)
            .Select(m => new {
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
        
        return new ChartScriptViewModel()
        {
            Labels = medicamentsLabels,
            Data = medicamentsData,
            Label = "Prescrits",
            ChartName = "medicamentsChart",
            ChartType = "pie",
            BorderColor = new[]
            {
                "rgba(255, 206, 86, 1)",
                "rgba(255, 159, 64, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(101, 179, 255, 1)",
                "rgba(153, 102, 255, 1)"
            },
            BackgroundColor = new[]
            {
                "rgba(255, 206, 86, 0.2)",
                "rgba(255, 159, 64, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(101, 179, 255, 0.2)",
                "rgba(153, 102, 255, 0.2)"
            }
        };
    }
    
    private ChartScriptViewModel MostCommonAllergiesStat()
    {
        var mostCommonAllergies = _dbContext.Allergies.Select(a => new
        {
            Allergy = a,
            Count = a.Patients.Count
        }).OrderByDescending(a => a.Count).Take(5).Select(a => a.Allergy)
            .Include(a => a.Patients).ToList();
        
        var allergiesLabels = new List<string>();
        var allergiesData = new List<int>();
        
        foreach (var allergy in mostCommonAllergies)
        {
            allergiesLabels.Add(allergy.Name);
            allergiesData.Add(allergy.Patients.Count);
        }

        return new ChartScriptViewModel()
        {
            Labels = allergiesLabels,
            Data = allergiesData,
            Label = "Allergie",
            ChartName = "allergiesChart",
            ChartType = "pie",
            BorderColor = new[]
            {
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)"
            },
            BackgroundColor = new[]
            {
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)"
            }
        };
    }
    
    private ChartScriptViewModel PatientsByAgeStat()
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
        
        return new ChartScriptViewModel()
        {
            Labels = patientsByAgeLabels,
            Data = patientsByAgeData,
            Label = "Patients",
            ChartName = "ageChart",
            ChartType = "bar",
            BorderColor = new[]
            {
                "rgba(101, 179, 255, 1)",
                "rgba(153, 102, 255, 1)",
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(201, 203, 207, 1)"
            },
            BackgroundColor = new[]
            {
                "rgba(101, 179, 255, 0.2)",
                "rgba(153, 102, 255, 0.2)",
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(201, 203, 207, 0.2)"
            }
        };
    }

    private ChartScriptViewModel MedicamentsByTypeStat()
    {
        var medicamentByType = _dbContext.Medicaments
            .GroupBy(m => m.Type)
            .OrderByDescending(g => g.Count()) // Trie les groupes par tailled
            .Select(g => new
            {
                Type = g.Key,
                Count = g.Count(),
                Medicaments = g.ToList()
            })
            .ToList();
        var patientsByGenderLabels = new List<string>();
        var patientsByGenderData = new List<int>();
        
        foreach (var group in medicamentByType)
        {
            patientsByGenderLabels.Add(group.Type.GetDisplayName());
            patientsByGenderData.Add(group.Count);
        }
        
        return new ChartScriptViewModel()
        {
            Labels = patientsByGenderLabels,
            Data = patientsByGenderData,
            Label = "Type",
            ChartName = "medicamentsTypeChart",
            ChartType = "bar",
            BorderColor = new[]
            {
                "rgba(255, 99, 132, 1)",
                "rgba(54, 162, 235, 1)",
                "rgba(255, 206, 86, 1)",
                "rgba(75, 192, 192, 1)",
                "rgba(153, 102, 255, 1)"
            },
            BackgroundColor = new[]
            {
                "rgba(255, 99, 132, 0.2)",
                "rgba(54, 162, 235, 0.2)",
                "rgba(255, 206, 86, 0.2)",
                "rgba(75, 192, 192, 0.2)",
                "rgba(153, 102, 255, 0.2)"
            }
        };
    }

    #endregion
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel();

        // données principales pour le tableau de bord
        model.Patients = _dbContext.Patients.Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.PatientId).Take(10).ToList();
        model.Prescriptions = _dbContext.Prescriptions.Where(p => p.DoctorId == UserId)
            .OrderByDescending(p => p.PrescriptionId).Take(10).ToList();
        
        model.TotalPatients = _dbContext.Patients.Count(p => p.DoctorId == UserId);
        model.TotalPrescriptions = _dbContext.Prescriptions.Count(p => p.DoctorId == UserId);
        
        var allergies = _dbContext.Allergies.Select(pa => pa.AllergyId).ToList();
        var medicalHistory = _dbContext.MedicalHistories.Select(mh => mh.MedicalHistoryId).ToList();
        var medicamentList = await _dbContext.Medicaments
            .Where(m => m.Allergies.Any(a => allergies.Contains(a.AllergyId))
                        || m.MedicalHistories.Any(mh => medicalHistory.Contains(mh.MedicalHistoryId)))
            .ToListAsync();
        
        model.TotalIncompatibilities = medicamentList.Count;
        model.TotalArchivedPrescriptions = _dbContext.Prescriptions.Count(p => p.DoctorId == UserId && p.EndDate < DateOnly.FromDateTime(DateTime.Now));
        
        // données pour les statistiques
        model.MostConsultedPatientsStatVm = MostConsultedPatientsStat();
        model.MostPrescribedMedicamentsStatVm = MostPrescribedMedicamentsStat();
        model.MostCommonAllergyStatVm = MostCommonAllergiesStat();
        model.PatientsByAgeStatVm = PatientsByAgeStat();
        model.MedicamentsByTypeStatVm = MedicamentsByTypeStat();
        
        TempData["SuccessMessage"] = "Bienvenue sur votre tableau de bord !";

        return View(model);
    }
}