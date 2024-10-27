namespace MedManager.ViewModel;

public class PatientListViewModel
{
    public int PatientId { get; set; }
    
    public List<Models.Patient> Patients { get; set; } = new();
}