using System.ComponentModel.DataAnnotations;

namespace MedManager.ViewModel.Account;

public class AccountViewModel
{
    public string? Id { get; set; }
    
    [Display(Name = "Adresse mail")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "L'adresse mail est requis.")]
    public string? Email { get; set; }
    
    [Display(Name = "Nom d'utilisateur")]
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string? Username { get; set; }
    
    [Display(Name = "Mot de passe")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Display(Name = "Prénom")]
    [Required(ErrorMessage = "Le prénom est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom de famille doit contenir entre 2 et 100 caractères.")]
    public string? FirstName { get; set; }
    
    [Display(Name = "Nom de famille")]
    [Required(ErrorMessage = "Le nom de famille est requis.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom de famille doit contenir entre 2 et 100 caractères.")]
    public string? LastName { get; set; }
    
    [Display(Name = "Adresse")]
    [Required(ErrorMessage = "L'adresse est requise.")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "L'adresse doit contenir entre 2 et 256 caractères.")]
    
    public string? Address { get; set; }
    
    [Display(Name = "Faculté")]
    [Required(ErrorMessage = "La faculté est requise.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "La faculté doit contenir entre 2 et 100 caractères.")]
    public string? Faculty { get; set; }
    
    [Display(Name = "Spécialité")]
    [Required(ErrorMessage = "La spécialité est requise.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "La spécialité doit contenir entre 2 et 100 caractères.")]
    public string? Specialty { get; set; }
}