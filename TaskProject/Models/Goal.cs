using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    public class Goal
    {
        public Goal()
        {
            TaskStart = DateTime.Now;
            Skills = new List<Skill>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterTaskId { get; set; }
        [Required]
        [Display(Name = "Название задачи")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Начало выполнения задачи")]
        public DateTime TaskStart { get; set; }
        [Display(Name = "Окончания задачи")]
        public DateTime? TaskEnd { get; set; } = null;
        [Display(Name = "Выполнение")]
        public bool IsComplete { get; set; } = false;


        [ForeignKey("Repeat")]
        public int RepeatId { get; set; }
        public virtual Repeat Repeat { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("Complication")]
        public int ComplicationId { get; set; }
        [Display(Name = "Сложность")]
        public virtual Complication Complication { get; set; }
        [Display(Name = "Навыки")]
        public virtual List<Skill> Skills { get; set; }
    }

    public class Repeat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RepeatId { get; set; }
        [Display(Name = "Повтор")]
        public string Name { get; set; }
    }

    public class Complication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ComplicationId { get; set; }
        [Display(Name = "Сложность")]
        public string Name { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int Damage { get; set; }
    }
}
