using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using TaskLibrary;
using TaskProject.Models;

namespace Spacewander.Areas.HabitsAndTasks.Controllers
{
    public class HabitsController : Controller
    {
        private ApplicationDbContext db;

        public ActionResult Index(ApplicationDbContext _db)
        {
            db = _db;
            var habits = db.Habits.Include(h => h.User).Include(h => h.Complication).Include(h => h.Atribute);
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
                if(!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
                {
                    return new StatusCodeResult(400);
                }
                string id = HttpContext.Request.Cookies["UserId"];

                habit.HabitStart = DateTime.Now.Date;
                habit.HabitEnd = DateTime.Now.Date;

                ApplicationUser User = db.Users.Where(c => c.Id == id).SingleOrDefault();

                User.Habits.Add(habit);
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
            Habit.User.CurrentExp = Habit.User.CurrentExp + Habit.Complication.Exp;
            Habit.User.CurrentGold = Habit.User.CurrentGold + Habit.Complication.Gold;

            Habit.User.CurrentHealth = Habit.User.CurrentHealth + Habit.Complication.Damage;
            if (Habit.User.CurrentHealth > Habit.User.MaxHealth)
            {
                Habit.User.CurrentHealth = Habit.User.MaxHealth;
            }

            if (Habit.User.CurrentExp >= Habit.User.MaxExp)
            {
                Habit.User.CurrentLevel++;
                Habit.User.CurrentExp = 0;
            }

            UserAtribute Atribute = Habit.User.Atributes.Where(s => s.AtributeId == Habit.AtributeId).FirstOrDefault();
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
