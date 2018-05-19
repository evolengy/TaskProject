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
            IsSetValue = false;

            Aligment = null;
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
        public bool IsSetValue { get; set; }

        public void SetDefaultValue()
        {
            this.Atributes = new List<Atribute>()
                {
                     new Atribute
                        {
                            Name = "Сила",
                            Description = "Характеристика, отвечающая за общее физическое развитие персонажа"
                        },
                        new Atribute
                        {
                            Name = "Здоровье",
                            Description = "Характеристика, отвечающая за физическое состояние персонажа"
                        },
                        new Atribute
                        {
                            Name = "Харизма",
                            Description = "Характеристика, отвечающая за навыки взаимодействия с другими людьми"
                        },
                        new Atribute
                        {
                            Name = "Профессионализм",
                            Description = "Характеристика, отвечающая за умственное развитие персонажа"
                        },
                        new Atribute
                        {
                            Name = "Интеллект",
                            Description = "Характеристика, отвечающая за умственное развитие персонажа"
                        },
                        new Atribute
                        {
                            Name = "Известность",
                            Description = "Характеристика, отвечающая за влияние персонажа в обществе"
                        },
                        new Atribute
                        {
                            Name = "Психика",
                            Description = "Характеристика, отвечающая за стрессоустойчивость персонажа и его психическое состояние"
                        }
                };
            this.Skills = new List<Skill>()
            {
                new Skill
                {
                    Name = "Владение холодным оружием",
                    Atribute = this.Atributes.Where(c=> c.Name == "Сила").Single()
                },
                new Skill
                {
                    Name = "Дрессировка собак",
                    Atribute = this.Atributes.Where(c => c.Name == "Профессионализм").Single()
                },
                new Skill
                {
                    Name = "Чтение",
                    Atribute = this.Atributes.Where(c => c.Name == "Интеллект").Single()
                }
            };
            this.Goals = new List<Goal>()
            {
                new Goal
                {
                    Name = "Сходить в кино с друзьями",
                    Description = "",
                    GoalEnd = DateTime.Now.AddDays(1),
                    RepeatId = 1,
                    ComplicationId = 1
                },
                new Goal
                {
                    Name = "Прочитать \" Горе от ума\"",
                    Description = "",
                    GoalEnd = DateTime.Now.AddDays(5),
                    RepeatId = 1,
                    ComplicationId = 2,
                    Skill = this.Skills.Where( c => c.Name == "Чтение").Single()
                }
            };
            this.IsSetValue = true;
        }
        public void CheckStatus()
        {
            foreach (var goal in this.Goals)
            {
                if (goal.GoalEnd < DateTime.Now && goal.IsComplete == false && (goal.GoalEnd.Value.Day - goal.GoalStart.Day) > TimeSpan.FromDays(1).Days)
                {
                    do
                    {
                        this.CurrentHealth = this.CurrentHealth - goal.Complication.Damage;
                        switch (goal.RepeatId)
                        {
                            case 2:
                                {
                                    goal.GoalEnd = null;
                                    break;
                                }
                            case 3:
                                {
                                    goal.GoalEnd = goal.GoalEnd.Value.AddDays(1);
                                    break;
                                }
                            case 5:
                                {
                                    goal.GoalEnd = goal.GoalEnd.Value.AddMonths(1);
                                    break;
                                }
                            case 6:
                                {
                                    goal.GoalEnd = goal.GoalEnd.Value.AddYears(1);
                                    break;
                                }
                        }
                    }
                    while (goal.GoalEnd < DateTime.Now);
                    CheckDead();
                }
            }
        }
        public void CheckLvl()
        {
            if (this.CurrentExp >= this.MaxExp)
            {
                CurrentLevel++;
                CurrentExp = 0;
            }
        }
        public void CheckDead()
        
        {
            if (this.IsDead || this.CurrentHealth <= 0)
            {
                this.CurrentHealth = 0;
                this.IsDead = true;
            }
        }
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

