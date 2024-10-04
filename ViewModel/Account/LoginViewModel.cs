using System.ComponentModel.DataAnnotations;

namespace MedManager.ViewModel.Account;

public class LoginViewModel
{
    [Display(Name = "Nom d'utilisateur")]
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string? Username { get; set; }
    
    [Display(Name = "Mot de passe")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    public string? Password { get; set; }
    
    [Display(Name = "Se souvenir de moi ?")]
    public bool RememberMe { get; set; }
}