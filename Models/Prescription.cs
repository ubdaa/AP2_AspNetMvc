namespace AP2_AspNetMvc.Models;

public class Prescription
{
    public int PrescriptionId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public required string Dosage { get; set; }
    public required string AdditionalInformation { get; set; }
    
    public int PatientId { get; set; }
    public required Patient Patient { get; set; }
    
    public int DoctorId { get; set; }
    public required Doctor Doctor { get; set; }

    public List<Medicament> Medicaments { get; set; } = new();
    public List<MedicalHistory> MedicalHistories { get; set; } = new();
}