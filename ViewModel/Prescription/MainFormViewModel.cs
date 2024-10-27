namespace MedManager.ViewModel.Prescription;

public class MainFormViewModel
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Dosage { get; set; }
    public string? AdditionalInformation { get; set; }
    
    
    public List<Models.Medicament> Medicaments { get; set; } = new();
}