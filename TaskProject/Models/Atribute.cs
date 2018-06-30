using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    public class Atribute
    {
        public Atribute()
        {
            Lvl = 1;
            CurrentExp = 0;
            MaxExp = 500;

            Skills = new List<Skill>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AtributeId { get; set; }
        [Display(Name = "Навык")]
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int Lvl { get; set; }

        public int CurrentExp { get; set; }
        public int MaxExp { get; set; }

        public virtual List<Skill> Skills { get; set; }

        public void ExpUp()
        {
            CurrentExp += 5;

            if (CurrentExp == MaxExp)
            {
                Lvl++;
                CurrentExp = 0;
                MaxExp += 10;
            }
        }


        public string GetImgPath()
        {
            string path;
            switch (Name)
            {
                case "Сила":
                    {
                        return path = "/img/atribute/shield.svg";
                    }
                case "Здоровье":
                    {
                        return path = "/img/atribute/pulse.svg";
                    }
                case "Интеллект":
                    {
                        return path = "/img/atribute/brain.svg";
                    }
                case "Харизма":
                    {
                        return path = "/img/atribute/chat.svg";
                    }
                case "Профессионализм":
                    {
                        return path = "/img/atribute/pencil-ruler-pen.svg";
                    }
                case "Известность":
                    {
                        return path = "/img/atribute/team.svg";
                    }
                case "Психика":
                    {
                        return path = "/img/atribute/sad.svg";
                    }

                default:
                    {
                        return path = null;
                    }
            }
        }
    }
}
