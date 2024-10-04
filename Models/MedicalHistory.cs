using System.ComponentModel.DataAnnotations;

namespace MedManager.Models;

public class MedicalHistory
{    
    [Display(Name = "Antécédent Id")]
    public int MedicalHistoryId { get; set; }
    
    [Display(Name = "Nom de l'antécédent")]
    [Required(ErrorMessage = "Le nom de l'antécédent est requis.")]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Le nom de l'antécédent doit contenir moins de 256 caractères.")]
    public required string Name { get; set; }

    public List<Patient> Patients { get; set; } = new();
    public List<Medicament> Medicaments { get; set; } = new();
}