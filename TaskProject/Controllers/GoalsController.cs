using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> usermanager;

        public GoalsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            usermanager = _userManager;
        }

        public async Task<IActionResult> Index(bool iscomplete = false)
        {
            ViewBag.Complete = true;

            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                return View("Index");
            }

            var goals = await db.Goals.Where(g => g.UserId == user.Id).Include(g => g.Repeat).Include(g => g.Complication).Include(g => g.Skill).ToListAsync();

            if (!iscomplete)
            {
                ViewBag.Complete = false;
                var completegoals = goals.Where(g => g.IsComplete == false);
                return View(goals.ToList());
            }
            
            return View(goals.ToList());
        }


        public async Task <IActionResult> AddGoal()
        {
            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = null });

            ViewBag.RepeatId = new SelectList(await db.Repeats.ToListAsync(), "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(await db.Complications.ToListAsync(), "ComplicationId", "Name");
            ViewBag.SkillId = skills;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGoal(Goal goal)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);

                if (user == null)
                {
                    return View("Index");
                }

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

                goal.UserId = user.Id;

                db.Goals.Add(goal);
                db.SaveChanges();

                return PartialView("_Success");
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = null });

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.SkillId = skills;
            return PartialView(goal);
        }

        public ActionResult EditGoal(int? id)
        {
            Goal goal = db.Goals.Find(id);

            if (goal == null)
            {
                return NotFound();
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = null });

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name", goal.RepeatId);
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.SkillId = skills;
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

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = null }); 

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.SkillId = skills;
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