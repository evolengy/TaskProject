using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskLibrary;

namespace TaskProject.Controllers
{
    public class GoalsController : Controller
    {
        private ApplicationDbContext db;

        public ActionResult Index(ApplicationDbContext _db,bool iscomplete = false)
        {
            db = _db;
            ViewBag.Complete = true;
            if (!iscomplete)
            {
                ViewBag.Complete = false;
                var goals = db.Goals.Where(g => g.IsComplete == false);
                return View(goals.ToList());
            }
            var completegoals = db.Goals;
            return View(completegoals.ToList());
        }


        public ActionResult AddGoal()
        {
                ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name");
                ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name");
                ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name");
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGoal(Goal goal)
        {
            if (ModelState.IsValid)
            {
                int result;
                if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
                {
                    return new StatusCodeResult(400);
                }
                int id = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());
                Character Character = db.Characters.Where(c => c.CharacterId == id).SingleOrDefault();
                goal.CharacterId = id;

                switch (goal.RepeatId)
                {
                    case 1:
                        {
                            break;
                        }
                    default:
                        {
                            goal.TaskEnd = goal.TaskStart;
                            break;
                        }
                }

                Character.Goals.Add(goal);
                db.SaveChanges();
                return PartialView("_Success");
            }
            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name", goal.AtributeId);
            return PartialView(goal);
        }

        public ActionResult EditGoal(int? id)
        {
                if (id == null)
                {
                    return new StatusCodeResult(400);
                }
                Goal goal = db.Goals.Find(id);

                if (goal == null)
                {
                    return NotFound();
                }
                ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name", goal.RepeatId);
                ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
                ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name", goal.AtributeId);
                return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGoal(Goal goal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GameRoom", "Home", new { area = "" });
            }
            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.AtributeId = new SelectList(db.Atributes, "AtributeId", "Name", goal.AtributeId);
            return View(goal);
        }

        [HttpPost]
        public ActionResult DeleteGoal(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return NotFound();
            }
            db.Goals.Remove(goal);
            db.SaveChanges();
            return null;
        }

        public ActionResult CompleteGoal(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return NotFound();
            }

            switch (goal.RepeatId)
            {
                case 1:
                    {
                        goal.IsComplete = true;
                        break;
                    }
                case 2:
                    {
                        goal.TaskEnd = goal.TaskEnd.Value.AddDays(1);
                        break;
                    }
                case 3:
                    {
                        goal.TaskEnd = goal.TaskEnd.Value.AddMonths(1);
                        break;
                    }
                case 4:
                    {
                        goal.TaskEnd = goal.TaskEnd.Value.AddYears(1);
                        break;
                    }
            }
            goal.Character.CurrentExp = goal.Character.CurrentExp + goal.Complication.Exp;
            goal.Character.CurrentGold = goal.Character.CurrentGold + goal.Complication.Gold;


            goal.Character.CurrentHealth = goal.Character.CurrentHealth + goal.Complication.Damage;
            if (goal.Character.CurrentHealth > goal.Character.MaxHealth)
            {
                goal.Character.CurrentHealth = goal.Character.MaxHealth;
            }

            if (goal.Character.CurrentExp >= goal.Character.MaxExp)
            {
                goal.Character.CurrentLevel++;
                goal.Character.CurrentExp = 0;
            }

            CharacterAtribute Atribute = goal.Character.Atributes.Where(s => s.AtributeId == goal.AtributeId).FirstOrDefault();
            if (Atribute != null)
            {
                Atribute.CurrentExp = Atribute.CurrentExp + goal.Complication.Exp;

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