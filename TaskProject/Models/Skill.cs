 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
 using TaskProject.Models.GoalModels;

namespace TaskProject.Models
{
    public class Skill
    {
        public Skill()
        {
            RatingId = 1;
            CurrentExp = 0;
            MaxExp = 100;
            Lvl = 1;

            Goals = new List<Goal>();
        }

        public delegate void SkillStateHandler(object sender, NotificationEventArgs e);

        public event SkillStateHandler LvlUp;

        public event SkillStateHandler RatingUp;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillId { get; set; }

        [Display(Name="Название навыка")]
        [Required(ErrorMessage = "Не указано название навыка")]
        public string Name { get; set; }
        [Display(Name="Текущий уровень")]
        public int Lvl { get; set; }

        [Display(Name="Текущие очки опыта")]
        public int CurrentExp { get; set; }
        [Display(Name = "Максимальные очки опыта")]
        public int MaxExp { get; set; }

        [ForeignKey("User")]
        [Display(Name="Пользователь")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Atribute")]
        [Display(Name ="Характеристика")]
        public int AtributeId { get; set; }
        public virtual Atribute Atribute { get; set; }

        [ForeignKey("Rating")]
        [Display(Name = "Рейтинг")]
        public int RatingId { get; set; }
        public virtual Rating Rating { get; set; }

        public List<Goal> Goals { get; set; }

        public void ExpUp()
        {
            CurrentExp += 5;
            Atribute.ExpUp();

            if(CurrentExp == MaxExp)
            {

                Lvl++;
                CurrentExp = 0;
                MaxExp += 10;

                LvlUp?.Invoke(this, new NotificationEventArgs($"Уровень навыка {Name} повышен: {Lvl} уровень", UserId));

                int ratingtemp = RatingId;

                CheckRating();

                if(ratingtemp != RatingId)
                {
                    RatingUp?.Invoke(this, new NotificationEventArgs($"Рейтинг у навыка {Name} повышен", UserId));
                }
            }
        }

        private void CheckRating()
        {
            if (Lvl <= 10)
            {
                RatingId = 1;
            }
            else if (Lvl > 10 && Lvl <= 20)
            {
                RatingId = 2;
            }
            else
            {
                RatingId = 3;
            }
        }
    }

    public class Rating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RatingId { get; set; }
        public string Name { get; set; }
    }
}
