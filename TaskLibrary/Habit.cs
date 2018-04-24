using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Habit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HabitId { get; set; }
        [Required]
        [Display(Name = "Название привычки")]
        public string Name { get; set; }
        [Display(Name = "Привычка устоялась")]
        public bool IsAccepted { get; set; } = false;
        [Display(Name = "Полезная привычка")]
        public bool IsUseful { get; set; }
        [Display(Name = "Число дней")]
        public int DayCount { get; set; } = 0;
        [Display(Name = "Предупреждения")]
        public int WarningCount { get; set; } = 0;
        [Display(Name = "Дата начала привычки"), Column(TypeName = "Date")]
        public DateTime HabitStart { get; set; }
        [Display(Name = "Дата следующей проверки привычки"), Column(TypeName = "Date")]
        public DateTime HabitEnd { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
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
