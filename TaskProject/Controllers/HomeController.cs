using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskLibrary;

namespace TaskProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private Character Character { get; set; }

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
            Character = db.Characters.Where(c => c.UserId == user.Id).FirstOrDefault();

            if (Character == null)
            {
                Character = new Character() { UserId = user.Id };
                db.Characters.Add(Character);
                db.SaveChanges();

                HttpContext.Response.Cookies["CharacterId"].Value = Character.CharacterId.ToString();
                ViewSetProfileModel view = new ViewSetProfileModel();
                return View("SetProfile", view);
            }

            CheckHabits();
            CheckGoals();

            if (Character.IsDead || Character.CurrentHealth <= 0)
            {
                Character.CurrentHealth = 0;
                Character.IsDead = true;
                db.SaveChanges();
            }

            Character.Class = GetClass(Character.Atributes);
            db.SaveChanges();


            HttpContext.Response.Cookies["CharacterId"].Value = Character.CharacterId.ToString();
            return View(Character);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetProfile(ViewSetProfileModel view)
        {
            if (ModelState.IsValid)
            {
                int result;
                if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
                {
                    return RedirectToAction("LogOff", "Account");
                }

                int id = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());

                    Character Character = db.Characters.Where(c => c.CharacterId == id).SingleOrDefault();
                    db.Entry(Character).State = EntityState.Modified;
                    Character.Age = view.Age;
                    Character.Weight = view.Weight;
                    Character.Growth = view.Growth;
                    Character.Sex = view.Sex;
                    db.SaveChanges();

                    List<Atribute> DefaultAtributes = db.Atributes.ToList();
                    List<CharacterAtribute> Atributes = new List<CharacterAtribute>();
                    foreach (var DefaultAtribute in DefaultAtributes)
                    {
                        if (DefaultAtribute.Name != "Без характеристики")
                        {
                            CharacterAtribute CharacterAtribute = new CharacterAtribute()
                            {
                                Atribute = DefaultAtribute,
                                AtributeId = DefaultAtribute.AtributeId

                            };
                            Atributes.Add(CharacterAtribute);
                        }
                    }
                    return View("SetAtributes", Atributes);
              
            }
            return View(view);
        }

        [HttpPost]
        public ActionResult SetAtributes(List<CharacterAtribute> Atributes)
        {
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
            {
                return RedirectToAction("LogOff", "Account");
            }

            foreach (CharacterAtribute Atribute in Atributes)
            {
                Atribute.CurrentExp = 0;
                Atribute.MaxExp = Atribute.Value * 1000;
            }
            int id = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());

                Character Character = db.Characters.Where(c => c.CharacterId == id).SingleOrDefault();
                db.Entry(Character).State = EntityState.Modified;
                Character.Atributes = Atributes;
                Character.Class = GetClass(Character.Atributes);
                db.SaveChanges();
            
            return RedirectToAction("GameRoom", "Home");
        }

        public ActionResult Dead()
        {
            return PartialView();
        }

        private Class GetClass(List<CharacterAtribute> Atributes)
        {
            Class Class = new Class();
            CharacterAtribute Strong = new CharacterAtribute();
            CharacterAtribute Charisma = new CharacterAtribute();
            CharacterAtribute Intelligence = new CharacterAtribute();
            CharacterAtribute Professionalism = new CharacterAtribute();
            CharacterAtribute Psychic = new CharacterAtribute();

            foreach (CharacterAtribute Atribute in Atributes)
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

        private void CheckHabits()
        {
            foreach (var Habit in Character.Habits)
            {
                while (Habit.HabitEnd < DateTime.Now.Date)
                {
                    Character.CurrentHealth = Character.CurrentHealth - Habit.Complication.Damage;
                    Habit.HabitEnd = Habit.HabitEnd.AddDays(1);
                    Habit.WarningCount++;

                    CharacterAtribute Atribute = Character.Atributes.Where(a => a.AtributeId == Habit.AtributeId).SingleOrDefault();
                    if (Atribute != null)
                    {
                        Atribute.CurrentExp = Atribute.CurrentExp - Habit.Complication.Exp;
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

                if (Habit.DayCount >= 21)
                {
                    Habit.IsAccepted = true;
                }

                if (Habit.WarningCount >= 3)
                {
                    Habit.DayCount = 0;
                    Habit.IsAccepted = false;
                }
                db.SaveChanges();
            }
        }

        private void CheckGoals()
        {
            foreach (var Goal in Character.Goals)
            {
                if (Goal.TaskEnd < DateTime.Now && Goal.IsComplete == false && (Goal.TaskEnd.Value.Day - Goal.TaskStart.Day) > TimeSpan.FromDays(1).Days)
                {
                    CharacterAtribute Atribute = Character.Atributes.Where(s => s.AtributeId == Goal.AtributeId).SingleOrDefault();
                    do
                    {
                        Character.CurrentHealth = Character.CurrentHealth - Goal.Complication.Damage;
                        if (Atribute != null)
                        {
                            Atribute.CurrentExp = Atribute.CurrentExp - Goal.Complication.Exp;
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

                        switch (Goal.RepeatId)
                        {
                            case 2:
                                {
                                    Goal.TaskEnd = null;
                                    break;
                                }
                            case 3:
                                {
                                    Goal.TaskEnd = Goal.TaskEnd.Value.AddDays(1);
                                    break;
                                }
                            case 5:
                                {
                                    Goal.TaskEnd = Goal.TaskEnd.Value.AddMonths(1);
                                    break;
                                }
                            case 6:
                                {
                                    Goal.TaskEnd = Goal.TaskEnd.Value.AddYears(1);
                                    break;
                                }
                        }
                    }
                    while (Goal.TaskEnd < DateTime.Now);
                    db.SaveChanges();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
