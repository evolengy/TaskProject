using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    public class Mood
    {
        public Mood()
        {
            Date = DateTime.Now.Date;

        }

        public int MoodId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public string LinkImg { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public void GetLink()
        {
            switch (Name)
            {
                case "Плохое":
                    {
                        LinkImg = "/img/moods/crying.svg";
                        break;
                    }
                case "Сонное":
                    {
                        LinkImg = "/img/moods/sleepy.svg";
                        break;
                    }
                case "Нормальное":
                    {
                        LinkImg = "/img/moods/normal.svg";
                        break;
                    }
                case "Веселое":
                    {
                        LinkImg = "/img/moods/laughing.svg";
                        break;
                    }
                case "Счастлоивое":
                    {
                        LinkImg = "/img/moods/happy.svg";
                        break;
                    }


            }
        }

        public static List<Mood> GetMoods()
        {
            List<Mood> moods = new List<Mood>();
            moods.AddRange(
                new List<Mood>
                {
                    new Mood
                    {
                        Name = "Плохое",
                        LinkImg = "/img/moods/crying.svg"
                    },
                    new Mood
                    {
                        Name = "Сонное",
                        LinkImg = "/img/moods/sleepy.svg"
                    },
                    new Mood
                    {
                        Name = "Нормальное",
                        LinkImg = "/img/moods/normal.svg"
                    },
                    new Mood
                    {
                        Name = "Веселое",
                        LinkImg = "/img/moods/laughing.svg"
                    },
                    new Mood
                    {
                        Name = "Отличное",
                        LinkImg = "/img/moods/happy.svg"
                    }
                });
            return moods;
        }
    }
}
