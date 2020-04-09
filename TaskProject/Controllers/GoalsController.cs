using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;
using TaskProject.Models.GoalModels;

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

        //Goals

        public async Task<IActionResult> GetGoals(int? catalogid = null, bool iscomplete = false)
        {
            ViewBag.Complete = true;

            var user = await usermanager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            List<Goal> goals = new List<Goal>();

            if (catalogid == null)
            {
                goals = await db.Goals.Where(g => g.UserId == user.Id).Include(g => g.Repeat).Include(g => g.Complication).Include(g => g.Skill).ToListAsync();
                ViewBag.BreadCrumb = "Все задачи";
                ViewBag.CatalogId = null;
            }
            else
            {
                Catalog catalog = db.Catalogs.Where(c => c.CatalogId == catalogid).SingleOrDefault();

                if (catalog == null)
                {
                    return RedirectToAction("GetGoals", catalogid = null);
                }

                goals = await db.Goals.Where(g => g.UserId == user.Id && g.CatalogId == catalogid).Include(g => g.Complication).Include(g => g.Skill).ToListAsync();
                ViewBag.BreadCrumb = catalog.Name;
                ViewBag.CatalogId = catalogid;
            }

            if (!iscomplete)
            {
                ViewBag.Complete = false;
                var completegoals = goals.Where(g => g.IsComplete == false);
                return View(completegoals.ToList());
            }
            return View(goals.ToList());
        }


        public async Task<IActionResult> StatGoals()
        {
            var user = await usermanager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            List<Goal> goals = await db.Goals.Where(g => g.UserId == user.Id).Include(g => g.Repeat).Include(g => g.Complication).Include(g => g.Skill).ToListAsync();

            ViewBag.BreadCrumb = "Статистика задач";
            return View(goals.ToList());
        }

        public async Task<IActionResult> AddGoal(int catalogid)
        {
            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.CatalogId
                = new SelectList(await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).OrderBy(c => c.Name).ToListAsync(), "CatalogId", "Name", db.Catalogs.Where(c => c.CatalogId == catalogid)).ToList();

            ViewBag.SkillId = skills;
            ViewBag.RepeatId = new SelectList(await db.Repeats.ToListAsync(), "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(await db.Complications.ToListAsync(), "ComplicationId", "Name");
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
                    return RedirectToAction("Logout", "Account");
                }

                goal.UserId = user.Id;
                await db.AddGoalAsync(goal);

                ViewBag.Message = "Задача успешно добавлена.";
                return PartialView("_Success");
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.RepeatId = new SelectList(await db.Repeats.ToListAsync(), "RepeatId", "Name", goal.RepeatId);
            ViewBag.ComplicationId = new SelectList(await db.Complications.ToListAsync(), "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.CatalogId = new SelectList(await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).ToListAsync(), "CatalogId", "Name", goal.CatalogId);
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

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)).OrderBy(s => s.Name), "SkillId", "Name", goal.SkillId).ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name", goal.RepeatId);
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.CatalogId = new SelectList(await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).OrderBy(c => c.Name).ToListAsync(), "CatalogId", "Name", goal.CatalogId);
            ViewBag.SkillId = skills;
            return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGoal(Goal goal)
        {

            if (ModelState.IsValid)
            {
                await db.EditGoalAsync(goal);

                ViewBag.Message = "Задача успешно изменена.";
                return PartialView("_Success");
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name", goal.RepeatId);
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.CatalogId = new SelectList(await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).ToListAsync(), "CatalogId", "Name", goal.CatalogId);
            ViewBag.SkillId = skills;
            return PartialView(goal);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGoal(int? id)
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

            await db.DeleteGoalAsync(goal);

            return Ok();
        }

        public async Task<IActionResult> CompleteGoal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Goal goal = await db.Goals.Where(g => g.GoalId == id)
                .Include(g => g.Skill)
                .Include(g => g.Skill.Atribute)
                .Include(g => g.Complication)
                .Include(g => g.Repeat)
                .SingleOrDefaultAsync();

            if (goal == null)
            {
                return NotFound();
            }

            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            await db.CompleteGoal(goal, user);

            ViewBag.Message = "Задача выполнена - " + goal.Name;
            return PartialView("_Success");
        }

        public async Task<IActionResult> RestoreGoal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Goal goal = await db.Goals.Where(g => g.GoalId == id)
                .Include(g => g.Skill)
                .Include(g => g.Skill.Atribute)
                .Include(g => g.Complication)
                .Include(g => g.Repeat)
                .SingleOrDefaultAsync();

            if (goal == null)
            {
                return NotFound();
            }

            await db.RestoreGoal(goal);

            ViewBag.Message = "Задача восстановлена - " + goal.Name;
            return PartialView("_Success");

        }

        private bool GoalExists(int id)
        {
            return db.Goals.Any(e => e.GoalId == id);
        }


        //Catalogs

        public IActionResult AddCatalog()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCatalog(Catalog catalog)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);
                if (user == null)
                {
                    RedirectToAction("Logout", "Account");
                }

                catalog.UserId = user.Id;
                await db.Catalogs.AddAsync(catalog);

 
                await db.Notifications.AddAsync(new Notification()
                {
                    
                    Name = "Каталог добавлен: " + catalog.Name,
                    DateCreate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now),
                    UserId = catalog.UserId
                });
                await db.SaveChangesAsync();

                ViewBag.Message = "Список успешно создан.";
                return PartialView("_Success");
            }
            return View(catalog);
        }

        public async Task<IActionResult> EditCatalog(int? id)
        {
            Catalog goal = await db.Catalogs.FindAsync(id);

            if (goal == null)
            {
                return NotFound();
            }

            return PartialView(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCatalog(Catalog catalog)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    db.Update(catalog);
                    await db.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogExists(catalog.CatalogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Message = "Список изменен.";
                return PartialView("_Success");
            }

            return PartialView(catalog);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCatalog(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Catalog catalog = await db.Catalogs.FindAsync(id);
            if (catalog == null)
            {
                return NotFound();
            }
            db.Catalogs.Remove(catalog);

            await db.Notifications.AddAsync(new Notification()
            {
                Name = "Каталог удален: " + catalog.Name,
                DateCreate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now),
                UserId = catalog.UserId
            });

            await db.SaveChangesAsync();
            return Ok();
        }



        private bool CatalogExists(int id)
        {
            return db.Catalogs.Any(e => e.CatalogId == id);
        }

        // Dispose

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