using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedManager.Models;

public enum Genders
{
    [Description("Male")]
    [Display(Name = "Homme")]
    Male,
    [Description("Female")]
    [Display(Name = "Femme")]
    Female,
    [Description("Other")]
    [Display(Name = "Autre")]
    Other
}

public class Patient
{
    [Display(Name = "Patient ID")]
    public int PatientId { get; set; }
    
    [Display(Name = "Prénom")]
    [Required(ErrorMessage = "Le prénom est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères.")]
    public required string FirstName { get; set; }
    
    [Display(Name = "Nom de famille")] 
    [Required(ErrorMessage = "Le nom de famille est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom de famille doit contenir entre 2 et 100 caractères.")]
    public required string LastName { get; set; }

    [Display(Name = "Date de naissance")] 
    [Required(ErrorMessage = "La date de naissance est requis.")]
    public required DateOnly BirthDate { get; set; }
    
    [Required(ErrorMessage = "L'âge du patient est requis.")]
    public required int Age { get; set; }
    
    [Display(Name = "Sexe")] 
    [Required(ErrorMessage = "Le genre du patient est requis.")]
    public required Genders Gender { get; set; }
    
    [Display(Name = "Taille")] 
    [Required(ErrorMessage = "La taille du patient est requis.")]
    public required int Height { get; set; }
    
    [Display(Name = "Poids")] 
    [Required(ErrorMessage = "Le poids du patient est requis.")]
    public required int Weight { get; set; }
    
    [Display(Name = "Adresse")]
    [Required(ErrorMessage = "L'adresse du patient est requis.")]
    [StringLength(512, MinimumLength = 1, ErrorMessage = "L'adresse du patient doit contenir moins de 512 caractères.")]
    public required string Address { get; set; }
    
    [Display(Name = "Numéro de sécurité social")] 
    [Required(ErrorMessage = "Le numéro de sécurité social est requis.")]
    [StringLength(18, MinimumLength = 13, ErrorMessage = "Le numéro de sécurité social doit contenir au moins 13 chiffres.")]
    [RegularExpression(@"^(1|2)\s?\d{2}\s?(0[1-9]|1[0-2])\s?(2[AB0-9]|9[0-69]\d|[013-9]\d)\s?\d{3}\s?\d{3}$")]
    public required string SocialSecurityNumber { get; set; }
    
    [Display(Name = "Date de création")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public required string DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public List<Prescription> Prescriptions { get; set; } = new();
    public List<Allergy> Allergies { get; set; } = new();
    public List<MedicalHistory> MedicalHistories { get; set; } = new();
}