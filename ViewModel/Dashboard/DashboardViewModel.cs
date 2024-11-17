namespace MedManager.ViewModel.Dashboard;

public class DashboardViewModel
{
    public List<Models.Prescription> Prescriptions { get; set; } = new();
    public List<Models.Patient> Patients { get; set; } = new();

    public int TotalPatients { get; set; } = 0;
    public int TotalPrescriptions { get; set; } = 0;
    public int TotalIncompatibilities { get; set; } = 0;
    public int TotalArchivedPrescriptions { get; set; } = 0;
    
    public ChartScriptViewModel MostConsultedPatientsStatVm { get; set; } = new();
    public ChartScriptViewModel MostPrescribedMedicamentsStatVm { get; set; } = new();
    public ChartScriptViewModel MostCommonAllergyStatVm { get; set; } = new();
    public ChartScriptViewModel PatientsByAgeStatVm { get; set; } = new();
    public ChartScriptViewModel MedicamentsByTypeStatVm { get; set; } = new();
}