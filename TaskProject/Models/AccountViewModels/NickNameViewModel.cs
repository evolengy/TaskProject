using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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