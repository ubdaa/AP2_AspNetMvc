using System.ComponentModel.DataAnnotations;
using MedManager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedManager.ViewModel.Medicament;

public class MedicamentViewModel
{
    public int MedicamentId { get; set; }
    
    [Display(Name = "Nom du médicament")]
    [Required(ErrorMessage = "Le nom du médicament est requis.")]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Le nom du médicament doit contenir moins de 256 caractères.")]
    public string Name { get; set; }
    
    [Display(Name = "Quantité du médicament")]
    [Required(ErrorMessage = "La quantité est requise.")]
    [StringLength(1024, MinimumLength = 1, ErrorMessage = "Le quantité du médicament doit contenir moins de 1024 caractères.")]
    public string Quantity { get; set; }
    
    [Display(Name = "Ingrédients du médicament")]
    [Required(ErrorMessage = "Les ingrédients sont requis.")]
    [StringLength(1024, MinimumLength = 1, ErrorMessage = "Les ingrédients du médicament doivent contenir moins de 1024 caractères.")]
    public string Ingredients { get; set; }
    
    [Display(Name = "Type de médicament")]
    [Required(ErrorMessage = "Le type de médicament est requis.")]
    public MedicamentTypes MedicamentType { get; set; }
    
    [Display(Name = "Catégorie de médicament")]
    [Required(ErrorMessage = "La catégorie de médicament est requise.")]
    public MedicamentCategories MedicamentCategory { get; set; }
    

    public List<int> SelectedAllergyIds { get; set; } = new();
    public List<int> SelectedMedicalHistoryIds { get; set; } = new();
    
    public List<SelectListItem> DrpAllergies { get; set; } = new();
    public List<SelectListItem> DrpMedicalHistories { get; set; } = new();
}