using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "{0} должен быть не менее {2} и максимум {1} длины.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Код аутентификации")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Запомнить эту машину")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
