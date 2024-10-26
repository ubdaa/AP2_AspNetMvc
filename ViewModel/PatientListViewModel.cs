using MedManager.Models;

public class PatientListViewModel
{
    public int PatientId { get; set; }
    
    public List<Patient> Patients { get; set; } = new();
}