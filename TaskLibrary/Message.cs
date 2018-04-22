using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Message
    {
        public int Id { get; set; }
        public string Theme { get; set; }
        [Required]
        [Display(Name = "Ваше имя")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Сообщение")]
        public string Body { get; set; }
        public string DateCreate { get; set; }
    }
}
