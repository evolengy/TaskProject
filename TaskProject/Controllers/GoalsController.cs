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

        public async Task<IActionResult> GetGoals(bool iscomplete = false)
        {
            ViewBag.Complete = true;

            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                return View("Index");
            }

            var goals = await db.Goals.Where(g => g.UserId == user.Id).Include(g => g.Repeat).Include(g => g.Complication).Include(g => g.Skill).ToListAsync();
            ViewBag.BreadCrumb = "Все задачи";

            if (!iscomplete)
            {
                ViewBag.Complete = false;
                var completegoals = goals.Where(g => g.IsComplete == false);
                return View(completegoals.ToList());
            }

            return View(goals.ToList());
        }


        public async Task<IActionResult> AddGoal()
        {
            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

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

                if (goal.SkillId == 0)
                {
                    goal.SkillId = null;
                }

                goal.UserId = user.Id;

                db.Goals.Add(goal);
                db.SaveChanges();

                ViewBag.Message = "Задача успешно добавлена.";
                return PartialView("_Success");
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.RepeatId = new SelectList(await db.Repeats.ToListAsync(), "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(await db.Complications.ToListAsync(), "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.SkillId = skills;

            return PartialView(goal);
        }

        public async Task<IActionResult> EditGoal(int? id)
        {
            Goal goal = await db.Goals.FindAsync(id);

            if (goal == null)
            {
                return NotFound();
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name", goal.SkillId).ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name", goal.RepeatId);
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.SkillId = skills;
            return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGoal(Goal goal)
        {
            if (ModelState.IsValid)
            {
                if (goal.SkillId == 0)
                {
                    goal.SkillId = null;
                }

                try
                {
                    db.Update(goal);
                    await db.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalExists(goal.GoalId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Message = "Задача успешно изменена.";
                return PartialView("_Success");
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.SkillId = skills;
            return PartialView(goal);
        }

        [HttpPost]
        public async Task <IActionResult> DeleteGoal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Goal goal = await db.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }
            db.Goals.Remove(goal);
            await db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> CompleteGoal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Goal goal = await db.Goals.Where(g => g.GoalId == id)
                .Include(g => g.User)
                .Include(g => g.Skill)
                .Include(g=>g.Skill.Atribute)
                .Include(g => g.Complication)
                .Include(g => g.Repeat)
                .SingleOrDefaultAsync();

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

            if (goal.Skill != null)
            {
                goal.Skill.ExpUp();
            }

            goal.User.CheckLvl();
            goal.User.CheckStatus();

            await db.SaveChangesAsync();

            ViewBag.Message = "Задача выполнена.";
            return PartialView("_Success");
        }


        private bool GoalExists(int id)
        {
            return db.Goals.Any(e => e.GoalId == id);
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