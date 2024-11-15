namespace MedManager.ViewModel.Search;

public class ResultViewModel
{
    public List<Models.Patient> Patients { get; set; } = new();
    public List<Models.Allergy> Allergies { get; set; } = new();
    public List<Models.MedicalHistory> MedicalHistories { get; set; } = new();
    public List<Models.Prescription> Prescriptions { get; set; } = new();
    public List<Models.Medicament> Medicaments { get; set; } = new();
}