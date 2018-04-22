using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Character
    {
        public Character()
        {
            Goals = new List<Goal>();
            Attainments = new List<Attainment>();
            Atributes = new List<CharacterAtribute>();
            Habits = new List<Habit>();
            Missions = new List<MissionsCondition>();
            GuildsRep = new List<GuildsReputation>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CharacterId { get; set; }

        public int Age { get; set; } = 0;
        public string Sex { get; set; }
        public int Growth { get; set; } = 0;
        public int Weight { get; set; } = 0;
        [Display(Name = "ИМТ")]
        public int IMT { get; set; } = 0;

        public long CurrentExp { get; set; } = 0;
        public int CurrentLevel { get; set; } = 1;

        public int CurrentGold { get; set; } = 0;
        public int CurrentHealth { get; set; } = 100;

        public long MaxExp { get; set; } = 500;
        public int MaxHealth { get; set; } = 100;

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Class")]
        public int? ClassId { get; set; }
        public Class Class { get; set; }

        public virtual List<Habit> Habits { get; set; }
        public virtual List<Goal> Goals { get; set; }
        public virtual List<Attainment> Attainments { get; set; }
        public virtual List<CharacterAtribute> Atributes { get; set; }
        public virtual List<MissionsCondition> Missions { get; set; }
        public virtual List<GuildsReputation> GuildsRep { get; set; }

        public bool IsDead { get; set; } = false;
    }

    public class ViewSetProfileModel
    {
        [Display(Name = "Ваш возраст: (в г.)"), Range(1, 150, ErrorMessage = "Укажите верный возраст")]
        public int Age { get; set; } = 1;
        [Display(Name = "Ваш пол:")]
        public string Sex { get; set; }
        [Display(Name = "Ваш рост (в см.)"), Range(1, 500, ErrorMessage = "Укажите верный рост")]
        public int Growth { get; set; } = 1;
        [Display(Name = "Ваш вес (в кг.)"), Range(1, 500, ErrorMessage = "Укажите верный вес")]
        public int Weight { get; set; } = 1;
        public SelectList SexSelect { get; set; } = new SelectList(
                    new List<SelectListItem>
                    {
                        new SelectListItem {Text = "Мужской", Value = "Мужчина"},
                        new SelectListItem {Text = "Женский", Value = "Женщина"}
                    }, "Value", "Text"
                    );
    }
}
