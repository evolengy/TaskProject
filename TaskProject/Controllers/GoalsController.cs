using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class GoalsController : Controller
    {
        private ApplicationDbContext db;

        public GoalsController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public ActionResult Index(bool iscomplete = false)
        {
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
                if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
                {
                    return new StatusCodeResult(400);
                }
                string id = HttpContext.Request.Cookies["UserId"];
                ApplicationUser user = db.Users.Where(c => c.Id == id).SingleOrDefault();
                goal.UserId = id;

                switch (goal.RepeatId)
                {
                    case 1:
                        {
                            break;
                        }
                    default:
                        {
                            goal.GoalEnd = goal.GoalStart;
                            break;
                        }
                }

                user.Goals.Add(goal);
                db.SaveChanges();
                return PartialView("_Success");
            }
            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
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
                        goal.GoalEnd = goal.GoalEnd.Value.AddDays(1);
                        break;
                    }
                case 3:
                    {
                        goal.GoalEnd = goal.GoalEnd.Value.AddMonths(1);
                        break;
                    }
                case 4:
                    {
                        goal.GoalEnd = goal.GoalEnd.Value.AddYears(1);
                        break;
                    }
            }
            goal.User.CurrentExp = goal.User.CurrentExp + goal.Complication.Exp;
            goal.User.CurrentGold = goal.User.CurrentGold + goal.Complication.Gold;


            goal.User.CurrentHealth = goal.User.CurrentHealth + goal.Complication.Damage;
            if (goal.User.CurrentHealth > goal.User.MaxHealth)
            {
                goal.User.CurrentHealth = goal.User.MaxHealth;
            }

            if (goal.User.CurrentExp >= goal.User.MaxExp)
            {
                goal.User.CurrentLevel++;
                goal.User.CurrentExp = 0;
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