namespace MedManager.ViewModel.Search;

public class ResultViewModel
{
    public List<Models.Patient> Patients { get; set; }
    public List<Models.Allergy> Allergies { get; set; }
    public List<Models.MedicalHistory> MedicalHistories { get; set; }
    public List<Models.Prescription> Prescriptions { get; set; }
    public List<Models.Medicament> Medicaments { get; set; }
}