using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AP2_AspNetMvc.Models;

public enum Genders
{
    [Description("Male")]
    [Display(Name = "Male")]
    Male,
    [Description("Female")]
    [Display(Name = "Female")]
    Female,
    [Description("Other")]
    [Display(Name = "Other")]
    Other
}

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
    
    [Required(ErrorMessage = "Age is required.")]
    public int Age { get; set; }
    
    [Required(ErrorMessage = "Gender is required.")]
    public Genders Gender { get; set; }
    
    [Required(ErrorMessage = "Height is required.")]
    public string? Height { get; set; }
    
    [Required(ErrorMessage = "Weight is required.")]
    public int Weight { get; set; }
}