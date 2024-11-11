namespace MedManager.ViewModel.Dashboard;

public class DashboardViewModel
{
    public List<Models.Prescription> Prescriptions { get; set; } = new();
    public List<Models.Patient> Patients { get; set; } = new();

    public int TotalPatients { get; set; } = 0;
    public int TotalPrescriptions { get; set; } = 0;
}