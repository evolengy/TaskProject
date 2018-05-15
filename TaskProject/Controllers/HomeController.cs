using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskProject;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public HomeController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            usermanager = _userManager;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("GameRoom");
            }
            return View();
        }

        public async Task<ActionResult> GameRoom()
        {

            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                return View("Index");
            }

            if (user.IsSetDescr == false)
            {
                user.Atributes = new List<Atribute>()
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
                user.Skills = new List<Skill>()
            {
                new Skill
                {
                    Name = "Владение холодным оружием",
                    Atribute = user.Atributes.Where(c=> c.Name == "Сила").Single()
                },
                new Skill
                {
                    Name = "Дрессировка собак",
                    Atribute = user.Atributes.Where(c => c.Name == "Профессионализм").Single()
                },
                new Skill
                {
                    Name = "Чтение",
                    Atribute = user.Atributes.Where(c => c.Name == "Интеллект").Single()
                }
            };
                user.Goals = new List<Goal>()
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
                    Skill = user.Skills.Where( c => c.Name == "Чтение").Single()
                }
            };
                user.Aligment = db.Aligments.Where(a => a.AligmentId == 5).Single();
                user.IsSetDescr = true;
                await db.SaveChangesAsync();
            }

            var dbuser = db.Users.Where(u => u.Id == user.Id).Include(u => u.Goals).Include(u => u.Skills).Include(u => u.Atributes).Include(u => u.Aligment).Single();

            //if (!user.IsSetDescr)
            //{
            //    ViewSetProfileModel view = new ViewSetProfileModel();

            //    List<Atribute> DefaultAtributes = db.Atributes.ToList();

            //    foreach (var DefaultAtribute in DefaultAtributes)
            //    {
            //            view.UserAtributes.Add(new UserAtribute()
            //            {
            //                Atribute = DefaultAtribute,
            //                AtributeId = DefaultAtribute.AtributeId,                          
            //            });
            //    }
            //    return View("SetProfile", view);
            //}

            CheckGoals(dbuser);

            if (dbuser.IsDead || dbuser.CurrentHealth <= 0)
            {
                dbuser.CurrentHealth = 0;
                dbuser.IsDead = true;
                db.SaveChanges();
            }

            return View(dbuser);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SetProfile(ViewSetProfileModel view)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await usermanager.GetUserAsync(User);
        //        if (user == null)
        //        {
        //            return View("Index");
        //        }

        //        foreach (UserAtribute atribute in view.UserAtributes)
        //        {
        //            atribute.MaxExp = atribute.Value * 1000;
        //        }

        //        db.Entry(user).State = EntityState.Modified;
        //        user.Age = view.Age;
        //        user.Weight = view.Weight;
        //        user.Growth = view.Growth;
        //        user.Sex = view.Sex;
        //        db.SaveChanges();

        //        return View("GameRoom","Home");

        //    }
        //    return View(view);
        //}

        public ActionResult Dead()
        {
            return PartialView();
        }

        private void CheckGoals(ApplicationUser user)
        {
            foreach (var goal in user.Goals)
            {
                if (goal.GoalEnd < DateTime.Now && goal.IsComplete == false && (goal.GoalEnd.Value.Day - goal.GoalStart.Day) > TimeSpan.FromDays(1).Days)
                {
                    do
                    {
                        user.CurrentHealth = user.CurrentHealth - goal.Complication.Damage;
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
                    db.SaveChanges();
                }
            }
        }
    }
}
