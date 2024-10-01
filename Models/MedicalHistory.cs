namespace AP2_AspNetMvc.Models;

public class MedicalHistory
{
    public int MedicalHistoryId { get; set; }
    public required string Name { get; set; }

    public List<Patient> Patients { get; set; } = new();
    public List<Medicament> Medicaments { get; set; } = new();
}