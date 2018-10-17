using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProject.Models
{
    public class Karma
    {
        public Karma()
        {
            Date = DateTime.Now;
        }

        public int KarmaId { get; set; }

        [Required(ErrorMessage = "Укажите название поступка")]
        [Display(Name = "Поступок")]
        public string Name { get; set; }
        [Display(Name = "Дата выполнения")]
        public DateTime Date { get; set; }
        [Display(Name = "Значение поступка")]
        public bool IsGood { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
