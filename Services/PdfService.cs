using System.Dynamic;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using MedManager.Models;
using MedManager.Utils;

namespace MedManager.Services;

public static class PdfService
{
    public static void GeneratePrescriptionPdf(string filePath, Prescription prescription)
    {
        using var writer = new PdfWriter(filePath);
        using var pdf = new PdfDocument(writer);
        
        // Créer un Document
        var document = new Document(pdf);
        
        var table = new Table(2, false);
        table.SetWidth(UnitValue.CreatePercentValue(100));
        table.SetBorder(Border.NO_BORDER);

        var leftCell = new Cell().SetBorder(Border.NO_BORDER);

        leftCell.Add(new Paragraph("Docteur")
            .SetFontSize(14)
            .SetBold()
            .SetMarginBottom(4));
        leftCell.Add(new Paragraph(prescription.Doctor.FullName)
            .SetFontSize(11));
        leftCell.Add(new Paragraph(prescription.Doctor.Email)
            .SetFontSize(11));
        
        var rightCell = new Cell().SetBorder(Border.NO_BORDER);
        rightCell.Add(new Paragraph("Patient")
            .SetFontSize(14)
            .SetBold()
            .SetMarginBottom(4));
        rightCell.Add(new Paragraph($"{prescription.Patient.FirstName} {prescription.Patient.LastName}, {prescription.Patient.Age} ans,")
            .SetFontSize(11));
        rightCell.Add(new Paragraph($"{prescription.Patient.Height} cm, {prescription.Patient.Weight} kg")
            .SetFontSize(11));
        rightCell.Add(new Paragraph(prescription.Patient.Address)
            .SetFontSize(11));
        rightCell.Add(new Paragraph(prescription.Patient.SocialSecurityNumber)
            .SetFontSize(11));
        
        table.AddCell(leftCell);
        table.AddCell(rightCell);
        
        document.Add(table);

        // Ajouter le titre de l'ordonnance
        document.Add(new Paragraph("Ordonnance Médicale")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(16)
            .SetBold()
            .SetMarginTop(15)
            .SetMarginBottom(20));

        // Ajouter la période de validité
        document.Add(new Paragraph($"Ordonnance valide du {prescription.StartDate?.ToString("dd/MM/yyyy") ?? "Non spécifiée"} au {prescription.EndDate?.ToString("dd/MM/yyyy") ?? "Non spécifiée"}")
            .SetFontSize(11)
            .SetItalic());

        // Ajouter les médicaments prescrits
        document.Add(new Paragraph("Médicaments :")
            .SetFontSize(14)
            .SetBold()
            .SetMarginTop(15)
            .SetMarginBottom(5));
        if (prescription.Medicaments.Any())
        {
            foreach (var medicament in prescription.Medicaments)
            {
                document.Add(new Paragraph($"- {medicament.Name} ({medicament.Category.GetDisplayName()}), {medicament.Type.GetDisplayName()}, {medicament.Quantity}")
                    .SetFontSize(11)
                    .SetMarginBottom(0));
                document.Add(new Paragraph($"{medicament.Ingredients}")
                    .SetFontSize(10)
                    .SetItalic()
                    .SetMarginBottom(5));
            }
        }
        else
        {
            document.Add(new Paragraph("Aucun médicament spécifié.")
                .SetFontSize(12)
                .SetMarginBottom(5));
        }

        if (!string.IsNullOrEmpty(prescription.Dosage))
        {
            document.Add(new Paragraph("Informations sur le dosage et la prise :")
                .SetFontSize(14)
                .SetBold()
                .SetMarginBottom(5)
                .SetMarginTop(10));
            document.Add(new Paragraph(prescription.Dosage)
                .SetFontSize(11)
                .SetMarginBottom(10));
        }

        // Ajouter les informations supplémentaires
        if (!string.IsNullOrEmpty(prescription.AdditionalInformation))
        {
            document.Add(new Paragraph("Informations supplémentaires :")
                .SetFontSize(14)
                .SetBold()
                .SetMarginBottom(5)
                .SetMarginTop(10));
            document.Add(new Paragraph(prescription.AdditionalInformation)
                .SetFontSize(11)
                .SetMarginBottom(10));
        }

        // Ajouter la date de création
        document.Add(new Paragraph($"Ordonnance générée le : {prescription.CreatedAt:dd/MM/yyyy}")
            .SetFontSize(10)
            .SetTextAlignment(TextAlignment.RIGHT));
        document.Add(new Paragraph($"MedManager")
            .SetFontSize(10)
            .SetItalic()
            .SetTextAlignment(TextAlignment.RIGHT));
        
        // Fermer le document
        document.Close();
    }
}