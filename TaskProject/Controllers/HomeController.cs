using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskLibrary;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public ActionResult Index(ApplicationDbContext _db)
        {
            db = _db;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("GameRoom");
            }
            return View();
        }

        public ActionResult GameRoom()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index");
            }

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (user.IsSetDescr)
            {

                Response.Cookies.Append("UserId" , user.Id.ToString());
                ViewSetProfileModel view = new ViewSetProfileModel();
                return View("SetProfile", view);
            }

            CheckHabits(user);
            CheckGoals(user);

            if (user.IsDead || user.CurrentHealth <= 0)
            {
                user.CurrentHealth = 0;
                user.IsDead = true;
                db.SaveChanges();
            }

            user.Class = GetClass(user.Atributes);
            db.SaveChanges();


            Response.Cookies.Append("UserId" , user.Id.ToString());
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetProfile(ViewSetProfileModel view)
        {
            if (ModelState.IsValid)
            {
                int result;
                if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
                {
                    return RedirectToAction("LogOff", "Account");
                }

                string id = HttpContext.Request.Cookies["UserId"];

                    ApplicationUser User = db.Users.Where(c => c.Id == id).SingleOrDefault();
                    db.Entry(User).State = EntityState.Modified;
                    User.Age = view.Age;
                    User.Weight = view.Weight;
                    User.Growth = view.Growth;
                    User.Sex = view.Sex;
                    db.SaveChanges();

                    List<Atribute> DefaultAtributes = db.Atributes.ToList();
                    List<UserAtribute> Atributes = new List<UserAtribute>();
                    foreach (var DefaultAtribute in DefaultAtributes)
                    {
                        if (DefaultAtribute.Name != "Без характеристики")
                        {
                            UserAtribute UserAtribute = new UserAtribute()
                            {
                                Atribute = DefaultAtribute,
                                AtributeId = DefaultAtribute.AtributeId

                            };
                            Atributes.Add(UserAtribute);
                        }
                    }
                    return View("SetAtributes", Atributes);
              
            }
            return View(view);
        }

        [HttpPost]
        public ActionResult SetAtributes(List<UserAtribute> Atributes)
        {
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
            {
                return RedirectToAction("LogOff", "Account");
            }

            foreach (UserAtribute Atribute in Atributes)
            {
                Atribute.CurrentExp = 0;
                Atribute.MaxExp = Atribute.Value * 1000;
            }
            string id = HttpContext.Request.Cookies["UserId"];

                ApplicationUser User = db.Users.Where(c => c.Id == id).SingleOrDefault();
                db.Entry(User).State = EntityState.Modified;
                User.Atributes = Atributes;
                User.Class = GetClass(User.Atributes);
                db.SaveChanges();
            
            return RedirectToAction("GameRoom", "Home");
        }

        public ActionResult Dead()
        {
            return PartialView();
        }

        private Class GetClass(List<UserAtribute> atributes)
        {
            Class Class = new Class();
            UserAtribute Strong = new UserAtribute();
            UserAtribute Charisma = new UserAtribute();
            UserAtribute Intelligence = new UserAtribute();
            UserAtribute Professionalism = new UserAtribute();
            UserAtribute Psychic = new UserAtribute();

            foreach (UserAtribute Atribute in atributes)
            {
                if (Atribute.AtributeId == 1)
                {
                    Strong = Atribute;
                }
                if (Atribute.AtributeId == 2)
                {
                    Charisma = Atribute;
                }
                if (Atribute.AtributeId == 3)
                {
                    Intelligence = Atribute;
                }
                if (Atribute.AtributeId == 4)
                {
                    Professionalism = Atribute;
                }
                if (Atribute.AtributeId == 5)
                {
                    Psychic = Atribute;
                }
            }
            if (Strong.Value <= 2 && Intelligence.Value <= 2 && Professionalism.Value <= 2 && Charisma.Value <= 2 && Psychic.Value <= 2)
            {
                Class =db.Classes.Find(10);
            }

            if (Strong.Value > 3 || Intelligence.Value > 3 || Professionalism.Value > 3 || Charisma.Value > 3 || Psychic.Value > 3)
            {
                Class = db.Classes.Find(6);
            }

            if (Strong.Value > 5 || Intelligence.Value > 5 || Professionalism.Value > 5 || Charisma.Value > 5 || Psychic.Value > 5)
            {
                Class = db.Classes.Find(2);
            }

            if (Strong.Value > 5 && Professionalism.Value > 5)
            {
                Class = db.Classes.Find(4);
            }

            if (Strong.Value > 7 && Intelligence.Value > 5 && Professionalism.Value > 5)
            {
                Class = db.Classes.Find(5);
            }

            if (Intelligence.Value > 7 && Professionalism.Value > 7)
            {
                Class = db.Classes.Find(1);
            }

            if (Strong.Value >= 9 && Professionalism.Value >= 9)
            {
                Class = db.Classes.Find(9);
            }

            if (Intelligence.Value >= 9 && Professionalism.Value >= 9)
            {
                Class = db.Classes.Find(3);
            }

            if (Charisma.Value >= 9 && Intelligence.Value >= 9)
            {
                Class = db.Classes.Find(11);
            }

            if (Strong.Value > 9 && Intelligence.Value > 9 && Professionalism.Value > 9)
            {
                Class = db.Classes.Find(7);
            }

            if (Strong.Value >= 9 && Intelligence.Value >= 9 && Professionalism.Value >= 9 && Charisma.Value >= 9 && Psychic.Value >= 9)
            {
                Class = db.Classes.Find(8);
            }
            return Class;
        }

        private void CheckHabits(ApplicationUser user)
        {
            foreach (var habit in user.Habits)
            {
                while (habit.HabitEnd < DateTime.Now.Date)
                {
                    user.CurrentHealth = user.CurrentHealth - habit.Complication.Damage;
                    habit.HabitEnd = habit.HabitEnd.AddDays(1);
                    habit.WarningCount++;

                    UserAtribute Atribute = user.Atributes.Where(a => a.AtributeId == habit.AtributeId).SingleOrDefault();
                    if (Atribute != null)
                    {
                        Atribute.CurrentExp = Atribute.CurrentExp - habit.Complication.Exp;
                        if (Atribute.CurrentExp < 0)
                        {
                            if (Atribute.Value > 0)
                            {
                                Atribute.Value--;
                            }
                            Atribute.CurrentExp = 0;
                            Atribute.MaxExp = Atribute.Value * 1000;
                        }
                    }
                }

                if (habit.DayCount >= 21)
                {
                    habit.IsAccepted = true;
                }

                if (habit.WarningCount >= 3)
                {
                    habit.DayCount = 0;
                    habit.IsAccepted = false;
                }
                db.SaveChanges();
            }
        }

        private void CheckGoals(ApplicationUser user)
        {
            foreach (var goal in user.Goals)
            {
                if (goal.TaskEnd < DateTime.Now && goal.IsComplete == false && (goal.TaskEnd.Value.Day - goal.TaskStart.Day) > TimeSpan.FromDays(1).Days)
                {
                    UserAtribute Atribute = user.Atributes.Where(s => s.AtributeId == goal.AtributeId).SingleOrDefault();
                    do
                    {
                        user.CurrentHealth = user.CurrentHealth - goal.Complication.Damage;
                        if (Atribute != null)
                        {
                            Atribute.CurrentExp = Atribute.CurrentExp - goal.Complication.Exp;
                            if (Atribute.CurrentExp < 0)
                            {
                                if (Atribute.Value > 0)
                                {
                                    Atribute.Value--;
                                }
                                Atribute.CurrentExp = 0;
                                Atribute.MaxExp = Atribute.Value * 1000;

                            }
                        }

                        switch (goal.RepeatId)
                        {
                            case 2:
                                {
                                    goal.TaskEnd = null;
                                    break;
                                }
                            case 3:
                                {
                                    goal.TaskEnd = goal.TaskEnd.Value.AddDays(1);
                                    break;
                                }
                            case 5:
                                {
                                    goal.TaskEnd = goal.TaskEnd.Value.AddMonths(1);
                                    break;
                                }
                            case 6:
                                {
                                    goal.TaskEnd = goal.TaskEnd.Value.AddYears(1);
                                    break;
                                }
                        }
                    }
                    while (goal.TaskEnd < DateTime.Now);
                    db.SaveChanges();
                }
            }
        }
    }
}
