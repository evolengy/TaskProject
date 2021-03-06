﻿using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models.ManageViewModels
{
	public class IndexViewModel
    {
        [Display(Name ="Учетная запись")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
