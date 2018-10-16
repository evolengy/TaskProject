using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
