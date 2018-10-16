using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
