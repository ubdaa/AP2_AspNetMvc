using System.ComponentModel.DataAnnotations;

namespace AP2_AspNetMvc.Models;

public class Doctor
{
    [Display(Name = "Doctor ID")]
    public int DoctorId { get; set; }
    
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last Name is required.")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Username is required.")]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }

    public List<Patient> Patients { get; set; } = new();
    public List<Prescription> Prescriptions { get; set; } = new();
    
    public string FullName => $"{FirstName} {LastName}";
}