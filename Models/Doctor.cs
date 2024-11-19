using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedManager.Models;

public class Doctor : IdentityUser
{
    [Display(Name = "Prénom")]
    [Required(ErrorMessage = "Le prénom est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères.")]
    public required string FirstName { get; set; }
    
    [Display(Name = "Nom de famille")] 
    [Required(ErrorMessage = "Le nom de famille est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom de famille doit contenir entre 2 et 100 caractères.")]
    public required string LastName { get; set; }
    
    [Display(Name = "Adresse")]
    [Required(ErrorMessage = "L'adresse est requise.")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "L'adresse doit contenir entre 2 et 256 caractères.")]
    public required string Address { get; set; }
    
    [Display(Name = "Faculté")]
    [Required(ErrorMessage = "La faculté est requise.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "La faculté doit contenir entre 2 et 100 caractères.")]
    public required string Faculty { get; set; }
    
    [Display(Name = "Spécialité")]
    [Required(ErrorMessage = "La spécialité est requise.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "La spécialité doit contenir entre 2 et 100 caractères.")]
    public required string Specialty { get; set; }

    public List<Patient> Patients { get; set; } = new();
    public List<Prescription> Prescriptions { get; set; } = new();
    
    public string FullName => $"{FirstName} {LastName}";
}