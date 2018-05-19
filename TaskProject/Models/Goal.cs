using Microsoft.EntityFrameworkCore;
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
            GoalStart = DateTime.Now;
            GoalEnd = null;
            IsComplete = false;

            SkillId = null;
            Skill = null;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalId { get; set; }
        [Required(ErrorMessage = "Имя не задано.")]
        [Display(Name = "Название задачи")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Начало задачи")]
        public DateTime GoalStart { get; set; }
        [Display(Name = "Окончания задачи")]
        public DateTime? GoalEnd { get; set; }

        [Display(Name = "Выполнение")]
        public bool IsComplete { get; set; } 


        [ForeignKey("Repeat")]
        public int RepeatId { get; set; }
        public Repeat Repeat { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Complication")]
        public int ComplicationId { get; set; }
        [Display(Name = "Сложность")]
        public virtual Complication Complication { get; set; }

        [ForeignKey("Skill")]
        public int? SkillId { get; set; }
        [Display(Name = "Навык")]
        public virtual Skill Skill { get; set; }
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
