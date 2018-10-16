using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models.GoalModels
{
    public class Goal
    {
        public Goal()
        {
            GoalEnd = null;
            IsComplete = false;
            FinishCount = 0;
            SeriesCount = 0;

            SkillId = null;
            Skill = null;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalId { get; set; }
        [Required(ErrorMessage = "Имя не задано.")]
        [Display(Name = "Название задачи"), MaxLength(50, ErrorMessage = "Превышено максимальное количество знаков в названии списка - 50")]
        public string Name { get; set; }
        [Display(Name = "Описание"), MaxLength(250, ErrorMessage = "Превышено максимальное количество знаков в описании задачи - 250")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Укажите дату начала задачи")]
        [Display(Name = "Начало задачи")]
        public DateTime? GoalStart { get; set; }
        [Display(Name = "Окончание задачи")]
        public DateTime? GoalEnd { get; set; }

        [Display(Name = "Выполнение")]
        public bool IsComplete { get; set; } 

        public int FinishCount { get; set; }
        public int SeriesCount { get; set; }
        public int PassCount { get; set; }

        [ForeignKey("Repeat")]
        public int RepeatId { get; set; }
        public Repeat Repeat { get; set; }

        [ForeignKey("Catalog")]
        public int CatalogId { get; set; }
        public virtual Catalog Catalog { get; set; }

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
    }
   
