using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskProject.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            CurrentLevel = 1;
            CurrentExp = 0;
            CurrentGold = 0;
            CurrentHealth = 100;
            Age = 0;
            Growth = 0;
            Weight = 0;
            IMT = 0;

            MaxExp = 500;
            MaxHealth = 100;

            IsDead = false;
            IsSetDescr = false;
        }

        public int Age { get; set; }
        public string Sex { get; set; }
        public float Growth { get; set; }
        public float Weight { get; set; }
        public float IMT { get; set; }

        public string Information { get; set; }

        public long CurrentExp { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentGold { get; set; }
        public int CurrentHealth { get; set; }
        public long MaxExp { get; set; }
        public int MaxHealth { get; set; }

        public Aligment Aligment { get; set; }

        public virtual List<Goal> Goals { get; set; }
        public virtual List<Atribute> Atributes { get; set; }
        public virtual List<Skill> Skills { get; set; }

        public bool IsDead { get; set; }
        public bool IsSetDescr { get; set; }
    }

    public class ViewSetProfileModel
    {
        public ViewSetProfileModel()
        {
            Atributes = new List<Atribute>();
        }

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

        public List<Atribute> Atributes { get; set; }
    }
}

