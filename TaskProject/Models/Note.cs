using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    public class Note
    {
        public Note()
        {
        }

        public int NoteId { get; set; }

        [Display(Name="Тема"),StringLength(50, ErrorMessage = "Название не больше 50 символов.")]
        public string Theme { get; set; }
        [Display(Name = "Текст")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Укажите дату создания заметки")]
        [Display(Name = "Дата")]
        public DateTime? DateCreate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
