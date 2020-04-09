using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models.AccountViewModels
{
	public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Введите вашу электронную почту")]
        [EmailAddress]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
    }
}
