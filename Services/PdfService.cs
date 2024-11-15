using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MedManager.Models;
namespace MedManager.Services;

public static class PdfService
{
    public static void GeneratePrescriptionPdf(string filePath, Prescription prescription)
    {
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        
        // Créer un Document
        var document = new Document(pdf);

        // Ajouter le titre de l'ordonnance
        document.Add(new Paragraph("Ordonnance Médicale")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20)
            .SetBold()
            .SetMarginBottom(20));

        // Ajouter les informations du médecin
        document.Add(new Paragraph($"Docteur : {prescription.Doctor.FullName}")
            .SetFontSize(12)
            .SetMarginBottom(5));
        document.Add(new Paragraph($"Contact : {prescription.Doctor.Email}")
            .SetFontSize(12)
            .SetMarginBottom(20));

        // Ajouter les informations du patient
        document.Add(new Paragraph($"Patient : {prescription.Patient.FirstName} {prescription.Patient.LastName}")
            .SetFontSize(12)
            .SetMarginBottom(5));
        document.Add(new Paragraph($"Date de naissance : {prescription.Patient.BirthDate:dd/MM/yyyy}")
            .SetFontSize(12)
            .SetMarginBottom(20));

        // Ajouter la période de validité
        document.Add(new Paragraph($"Date de début : {prescription.StartDate?.ToString("dd/MM/yyyy") ?? "Non spécifiée"}")
            .SetFontSize(12)
            .SetMarginBottom(5));
        document.Add(new Paragraph($"Date de fin : {prescription.EndDate?.ToString("dd/MM/yyyy") ?? "Non spécifiée"}")
            .SetFontSize(12)
            .SetMarginBottom(20));

        // Ajouter les médicaments prescrits
        document.Add(new Paragraph("Médicaments :")
            .SetFontSize(14)
            .SetBold()
            .SetMarginBottom(10));
        if (prescription.Medicaments.Any())
        {
            foreach (var medicament in prescription.Medicaments)
            {
                document.Add(new Paragraph($"- {medicament.Name} : {medicament.Quantity}")
                    .SetFontSize(12)
                    .SetMarginBottom(5));
            }
        }
        else
        {
            document.Add(new Paragraph("Aucun médicament spécifié.")
                .SetFontSize(12)
                .SetMarginBottom(10));
        }

        // Ajouter les informations supplémentaires
        if (!string.IsNullOrEmpty(prescription.AdditionalInformation))
        {
            document.Add(new Paragraph("Informations supplémentaires :")
                .SetFontSize(14)
                .SetBold()
                .SetMarginBottom(10));
            document.Add(new Paragraph(prescription.AdditionalInformation)
                .SetFontSize(12)
                .SetMarginBottom(20));
        }

        // Ajouter la date de création
        document.Add(new Paragraph($"Ordonnance générée le : {prescription.CreatedAt:dd/MM/yyyy}")
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.RIGHT));

        // Fermer le document
        document.Close();
    }
}