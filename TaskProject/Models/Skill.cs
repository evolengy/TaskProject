using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SkillId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string Name { get; set; }
        public int Lvl { get; set; }

        public int CurrentExp { get; set; }
        public int MaxExp { get; set; }

        public virtual Atribute Atribute { get; set; }

        [ForeignKey("Rating")]
        public int RatingId { get; set; }
        public virtual Rating Rating { get; set; }

        private void CheckRating()
        {
            if(Lvl <= 10)
            {
                RatingId = 1;
            }
            else if(Lvl > 10 && Lvl <= 20)
            {
                RatingId = 2;
            }
            else
            {
                RatingId = 3;
            }
        }

        public void CheckLvl()
        {
            if(CurrentExp == MaxExp)
            {
                Lvl++;
                CurrentExp = 0;
                MaxExp += 10;

                CheckRating();
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
