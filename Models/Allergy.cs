using System.ComponentModel.DataAnnotations;

namespace AP2_AspNetMvc.Models;

public class Allergy
{
    [Display(Name = "Allergie Id")]
    public int AllergyId { get; set; }
    
    [Display(Name = "Nom de l'allergie")]
    [Required(ErrorMessage = "Le nom de l'allergie est requis.")]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Le nom de l'allergie doit contenir moins de 256 caractères.")]
    public required string Name { get; set; }

    public List<Patient> Patients { get; set; } = new();
    public List<Medicament> Medicaments { get; set; } = new();
}