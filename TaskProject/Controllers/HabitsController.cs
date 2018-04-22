using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using TaskLibrary;

namespace Spacewander.Areas.HabitsAndTasks.Controllers
{
    public class HabitsController : Controller
    {
        private ApplicationDbContext db;

        public ActionResult Index(ApplicationDbContext _db)
        {
            db = _db;
            var habits = db.Habits.Include(h => h.Character).Include(h => h.Complication).Include(h => h.Atribute);
            return View(habits.ToList());
        }

        public ActionResult AddHabit()
        {
                ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name");
                ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name");
                return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddHabit(Habit habit)
        {
            if (ModelState.IsValid)
            {
                int result;
                if(!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
                {
                    return new StatusCodeResult(400);
                }
                int id = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());
                habit.HabitStart = DateTime.Now.Date;
                habit.HabitEnd = DateTime.Now.Date;
                Character Character = db.Characters.Where(c => c.CharacterId == id).SingleOrDefault();
                Character.Habits.Add(habit);
                db.SaveChanges();
                return RedirectToAction("GameRoom", "Home", new { area = "" });
            }

            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", habit.ComplicationId);
            ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name", habit.AtributeId);
            return PartialView(habit);
        }

        public ActionResult EditHabit(int? id)
        {
                if (id == null)
                {
                    return new StatusCodeResult(400);
                }
                Habit habit = db.Habits.Find(id);
                if (habit == null)
                {
                    return NotFound();
                }

                ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", habit.ComplicationId);
                ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name", habit.AtributeId);
                return PartialView(habit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHabit(Habit habit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(habit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GameRoom", "Home", new { area = "" });
            }
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", habit.ComplicationId);
            ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name", habit.AtributeId);
            return PartialView(habit);
        }

        [HttpPost]
        public ActionResult DeleteHabit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            Habit habit = db.Habits.Find(id);
            if (habit == null)
            {
                return NotFound();
            }
            db.Habits.Remove(habit);
            db.SaveChanges();
            return null;
        }

        public ActionResult CompleteHabit(int? id)
        {
            if(id == null)
            {
                return new StatusCodeResult(400);
            }
            Habit Habit = db.Habits.Find(id);
            if (Habit == null)
            {
                return NotFound();
            }

            Habit.DayCount++;
            Habit.HabitEnd = Habit.HabitEnd.AddDays(1);
            Habit.WarningCount = 0;

            if(Habit.DayCount >= 21)
            {
                Habit.IsAccepted = true;
            }
            Habit.Character.CurrentExp = Habit.Character.CurrentExp + Habit.Complication.Exp;
            Habit.Character.CurrentGold = Habit.Character.CurrentGold + Habit.Complication.Gold;

            Habit.Character.CurrentHealth = Habit.Character.CurrentHealth + Habit.Complication.Damage;
            if (Habit.Character.CurrentHealth > Habit.Character.MaxHealth)
            {
                Habit.Character.CurrentHealth = Habit.Character.MaxHealth;
            }

            if (Habit.Character.CurrentExp >= Habit.Character.MaxExp)
            {
                Habit.Character.CurrentLevel++;
                Habit.Character.CurrentExp = 0;
            }

            CharacterAtribute Atribute = Habit.Character.Atributes.Where(s => s.AtributeId == Habit.AtributeId).FirstOrDefault();
            if(Atribute != null)
            {
                Atribute.CurrentExp = Atribute.CurrentExp + Habit.Complication.Exp;

                if (Atribute.CurrentExp > Atribute.MaxExp)
                {
                    Atribute.Value++;
                    Atribute.CurrentExp = 0;
                    Atribute.MaxExp = Atribute.Value * 1000;
                }    
            }
            db.SaveChanges();
            return RedirectToAction("GameRoom", "Home", new { area = "" });
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
