using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AP2_AspNetMvc.Models;

public class Doctor : IdentityUser
{
    [Display(Name = "Doctor ID")]
    public int DoctorId { get; set; }
    
    [Display(Name = "Prénom")]
    [Required(ErrorMessage = "Le prénom est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères.")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Nom de famille")] 
    [Required(ErrorMessage = "Le nom de famille est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom de famille doit contenir entre 2 et 100 caractères.")]
    public string? LastName { get; set; }

    public List<Patient> Patients { get; set; } = new();
    public List<Prescription> Prescriptions { get; set; } = new();
    
    public string FullName => $"{FirstName} {LastName}";
}