using System.ComponentModel.DataAnnotations;

namespace MedManager.Models;

public class Prescription
{
    [Display(Name = "Prescription Id")]
    public int PrescriptionId { get; set; }
    
    [Display(Name = "Date de début")]
    public DateOnly? StartDate { get; set; }
    
    [Display(Name = "Date de Fin")]
    public DateOnly? EndDate { get; set; }
    
    [Display(Name = "Posologie")] 
    [StringLength(256, MinimumLength = 0, ErrorMessage = "La posologie doit contenir moins de 256 caractères.")]
    public string? Dosage { get; set; }
    
    [Display(Name = "Informations supplémentaires")] 
    [StringLength(2048, MinimumLength = 0, ErrorMessage = "La posologie doit contenir moins de 2048 caractères.")]
    public string? AdditionalInformation { get; set; }
    
    public int PatientId { get; set; }
    public required Patient Patient { get; set; }
    
    public required string DoctorId { get; set; }
    public required Doctor Doctor { get; set; }

    public List<Medicament> Medicaments { get; set; } = new();
}