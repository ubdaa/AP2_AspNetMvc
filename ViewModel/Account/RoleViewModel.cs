using System.ComponentModel.DataAnnotations;

namespace MedManager.ViewModel.Account;

public class RoleViewModel
{
    [Display(Name = "Rôle")]
    [Required(ErrorMessage = "Le rôle est requis.")]
    public Roles Role { get; set; }
}