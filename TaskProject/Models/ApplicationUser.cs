﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TaskProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            CurrentLevel = 1;
            MaxExp = 500;

            IsSetDefaultValues = false;

            AligmentId = null;

            Goals = new List<Goal>();
            Catalogs = new List<Catalog>();
            Atributes = new List<Atribute>();
            Skills = new List<Skill>();
            UserMoods = new List<UserMood>();
            UserAchievements = new List<UserAchievement>();
            UserRewards = new List<UserReward>();
            Notes = new List<Note>();
            Notifications = new List<Notification>();
        }

        public long CurrentExp { get; set; }
        public long MaxExp { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentGold { get; set; }

        [ForeignKey("Aligment")]
        public int? AligmentId { get; set; }
        public Aligment Aligment { get; set; }

        [ForeignKey("Health")]
        public int? HealthId { get; set; }
        public Health Health { get; set; }

        public virtual List<Goal> Goals { get; set; }
        public virtual List<Catalog> Catalogs { get; set; }
        public virtual List<Atribute> Atributes { get; set; }
        public virtual List<Skill> Skills { get; set; }
        public virtual List<UserMood> UserMoods { get; set; }
        public virtual List<UserAchievement> UserAchievements { get; set; }
        public virtual List<UserReward> UserRewards { get; set; }
        public virtual List<Note> Notes { get; set; }
        public virtual List<Notification> Notifications { get; set; }

        public bool IsSetDefaultValues { get; set; }
        public void SetDefaultValues()
        {
            Health = new Health() {
                UserId = this.Id
            };

            Catalog DefaultCatalog = new Catalog()
            {
                Name = "Мои задачи"
            };
            this.Atributes.AddRange(new List<Atribute>()
                {
                     new Atribute
                        {
                            Name = "Сила",
                            Description = "Характеристика, отвечающая за общее физическое развитие персонажа"
                        },
                        new Atribute
                        {
                            Name = "Здоровье",
                            Description = "Характеристика, отвечающая за поддержку физического самочувствия персонажа"
                        },
                        new Atribute
                        {
                            Name = "Харизма",
                            Description = "Характеристика, отвечающая за навыки взаимодействия с другими людьми"
                        },
                        new Atribute
                        {
                            Name = "Профессионализм",
                            Description = "Характеристика, отвечающая за развитие профессиональных качеств персонажа"
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
                });
            this.Skills.AddRange(new List<Skill>()
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
            });
            this.Catalogs.Add(DefaultCatalog);
            this.Goals.AddRange(new List<Goal>()
            {
                new Goal
                {
                    Name = "Сходить в кино с друзьями",
                    Description = "",
                    GoalEnd = DateTime.Now.AddDays(1),
                    RepeatId = 1,
                    ComplicationId = 1,
                    Catalog = DefaultCatalog
                    
                },
                new Goal
                {
                    Name = "Прочитать \" Горе от ума\"",
                    Description = "",
                    GoalEnd = DateTime.Now.AddDays(5),
                    RepeatId = 1,
                    ComplicationId = 2,
                    Skill = this.Skills.Where( c => c.Name == "Чтение").Single(),
                    Catalog = DefaultCatalog
                }
            });

            IsSetDefaultValues = true;
        }

        //public void RefreshStatus()
        //{
        //    foreach (var goal in this.Goals)
        //    {
        //        if (goal.GoalEnd < DateTime.Now && goal.IsComplete == false && (goal.GoalEnd.Value.Day - goal.GoalStart.Day) > TimeSpan.FromDays(1).Days)
        //        {
        //            do
        //            {
        //                switch (goal.RepeatId)
        //                {
        //                    case 2:
        //                        {
        //                            goal.GoalEnd = null;
        //                            break;
        //                        }
        //                    case 3:
        //                        {
        //                            goal.GoalEnd = goal.GoalEnd.Value.AddDays(1);
        //                            break;
        //                        }
        //                    case 5:
        //                        {
        //                            goal.GoalEnd = goal.GoalEnd.Value.AddMonths(1);
        //                            break;
        //                        }
        //                    case 6:
        //                        {
        //                            goal.GoalEnd = goal.GoalEnd.Value.AddYears(1);
        //                            break;
        //                        }
        //                }
        //            }
        //            while (goal.GoalEnd < DateTime.Now);
        //        }
        //    }
        //}

        public void CheckLvl()
        {
            if (this.CurrentExp >= this.MaxExp)
            {
                CurrentLevel++;
                CurrentExp = 0;
            }
        }
    }
}

