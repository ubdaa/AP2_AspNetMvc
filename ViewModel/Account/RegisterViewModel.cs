using System.ComponentModel.DataAnnotations;

namespace MedManager.ViewModel.Account;

public class RegisterViewModel
{
    [Display(Name = "Adresse mail")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "L'adresse mail est requis.")]
    public string? Email { get; set; }
    
    [Display(Name = "Nom d'utilisateur")]
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string? Username { get; set; }
    
    [Display(Name = "Mot de passe")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    public string? Password { get; set; }
    
    [Display(Name = "Prénom")]
    [Required(ErrorMessage = "Le prénom est requis.")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Nom de famille")]
    [Required(ErrorMessage = "Le nom de famille est requis.")]
    public string? LastName { get; set; }
}