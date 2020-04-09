using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models.AccountViewModels
{
	public class NickNameViewModel
    {
        [Required(ErrorMessage = "Неверно набран ник")]
        [DataType(DataType.Text)]
        [Display(Name = "Ник")]
        public string NickName { get; set; }
    }
}