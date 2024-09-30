using System.ComponentModel.DataAnnotations;

namespace AP2_AspNetMvc.Models;

public class Patient
{
    [Display(Name = "Patient ID")]
    public int PatientId { get; set; }
    
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Last Name")] 
    [Required(ErrorMessage = "Last Name is required.")]
    public string? LastName { get; set; }
    
    public int Age { get; set; }
    
    public int Weight { get; set; }
}