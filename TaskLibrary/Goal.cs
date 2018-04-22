using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Goal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterTaskId { get; set; }
        [Required]
        [Display(Name = "Название задачи")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Начало выполнения задачи")]
        public DateTime TaskStart { get; set; } = DateTime.Now;
        [Display(Name = "Окончания задачи")]
        public DateTime? TaskEnd { get; set; } = null;
        [Display(Name = "Выполнение")]
        public bool IsComplete { get; set; } = false;


        [ForeignKey("Repeat")]
        public int RepeatId { get; set; }
        public virtual Repeat Repeat { get; set; }
        [ForeignKey("Character")]
        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }
        [ForeignKey("Complication")]
        public int ComplicationId { get; set; }
        [Display(Name = "Сложность")]
        public virtual Complication Complication { get; set; }
        [ForeignKey("Atribute")]
        public int AtributeId { get; set; }
        [Display(Name = "Навык")]
        public virtual Atribute Atribute { get; set; }
    }
}
