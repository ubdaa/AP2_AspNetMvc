namespace AP2_AspNetMvc.Models;

public class Medicament
{
    public int MedicamentId { get; set; }
    public required string Nom { get; set; }
    public required string Quantité { get; set; }
    public required string Ingredients { get; set; }

    public List<Prescription> Prescriptions { get; set; } = new();
    public List<Allergy> Allergies { get; set; } = new();
    public List<MedicalHistory> MedicalHistories { get; set; } = new();
}