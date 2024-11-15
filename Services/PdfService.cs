using iText.Kernel.Pdf;
using MedManager.Models;

namespace MedManager.Services;

public class PdfService
{
    public static void GeneratePrescriptionPdf(string filePath, Prescription prescription)
    {
        using (var writer = new PdfWriter(filePath))
        {
            
        }
    }
}