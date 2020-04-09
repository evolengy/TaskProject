using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models.AccountViewModels
{
	public class LoginWithRecoveryCodeViewModel
    {
            [Required(ErrorMessage = "Введите код восстановления")]
            [DataType(DataType.Text)]
            [Display(Name = "Код восстановления")]
            public string RecoveryCode { get; set; }
    }
}
