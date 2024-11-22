using System.ComponentModel.DataAnnotations;
using MedManager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedManager.ViewModel.Patient;

public class PatientViewModel
{
    public int PatientId { get; set; } = 0;
    
    [Display(Name = "Prénom")] 
    [Required(ErrorMessage = "Le prénom est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères.")]
    public string FirstName { get; set; }
    
    [Display(Name = "Nom de famille")] 
    [Required(ErrorMessage = "Le nom de famille est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom de famille doit contenir entre 2 et 100 caractères.")]
    public string LastName { get; set; }

    [Display(Name = "Date de naissance")] 
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "La date de naissance est requis.")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly BirthDate { get; set; }
    
    [Display(Name = "Sexe")] 
    [Required(ErrorMessage = "Le genre du patient est requis.")]
    public Genders Gender { get; set; }
    
    [Display(Name = "Hauteur (en cm)")] 
    [Required(ErrorMessage = "La taille du patient est requis.")]
    [Range(typeof(int), "0", "300", ErrorMessage = "La taille du patient doit être comprise entre 0 et 300 cm.")]
    public int Height { get; set; }
    
    [Display(Name = "Poids (en kg arrondi à l'unité)")] 
    [Required(ErrorMessage = "Le poids du patient est requis.")]
    [Range(typeof(int), "0", "300", ErrorMessage = "Le poids du patient doit être compris entre 0 et 300 kg.")]
    public int Weight { get; set; }
    
    [Display(Name = "Adresse")]
    [Required(ErrorMessage = "L'adresse du patient est requis.")]
    [StringLength(512, MinimumLength = 1, ErrorMessage = "L'adresse du patient doit contenir moins de 512 caractères.")]
    public string Address { get; set; }
    
    [Display(Name = "Numéro de sécurité social")] 
    [Required(ErrorMessage = "Le numéro de sécurité social est requis.")]
    [StringLength(18, MinimumLength = 13, ErrorMessage = "Le numéro de sécurité social doit contenir au moins 13 chiffres.")]
    [RegularExpression(@"^(1|2)\s?\d{2}\s?(0[1-9]|1[0-2])\s?(2[AB0-9]|9[0-69]\d|[013-9]\d)\s?\d{3}\s?\d{3}$", ErrorMessage = "Votre numéro de sécurité social n'est pas valide.")]
    public string SocialSecurityNumber { get; set; }
    
    public List<Allergy> Allergies { get; set; } = new();
    public List<MedicalHistory> MedicalHistories { get; set; } = new();

    public List<int> SelectedAllergyIds { get; set; } = new();
    public List<int> SelectedMedicalHistoryIds { get; set; } = new();
    
    public List<SelectListItem> DrpAllergies { get; set; } = new();
    public List<SelectListItem> DrpMedicalHistories { get; set; } = new();
}