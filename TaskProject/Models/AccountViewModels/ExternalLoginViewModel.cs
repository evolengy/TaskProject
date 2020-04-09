using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models.AccountViewModels
{
	public class ExternalLoginViewModel
    {
        [Required(ErrorMessage = "¬ведите вашу электронную почту")]
        [EmailAddress]
        [Display(Name = "Ёлектронна€ почта")]
        public string Email { get; set; }
    }
}
