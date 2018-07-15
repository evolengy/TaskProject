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

        //Goals

        public async Task<IActionResult> GetGoals(int? catalogid = null , bool iscomplete = false )
        {
            ViewBag.Complete = true;

            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                return View("Index");
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

                if(catalog == null)
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

        public async Task<IActionResult> AddGoal(int catalogid)
        {
            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            var catalogs = await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).ToListAsync();

            ViewBag.RepeatId = new SelectList(await db.Repeats.ToListAsync(), "RepeatId", "Name");
            ViewBag.ComplicationId = new SelectList(await db.Complications.ToListAsync(), "ComplicationId", "Name");
            ViewBag.CatalogId = new SelectList(catalogs, "CatalogId", "Name", catalogs.Where(c => c.CatalogId == catalogid));
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
                    return RedirectToAction("Index","Home");
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

                await db.Goals.AddAsync(goal);

                await db.Notifications.AddAsync(new Notification()
                {
                    Name = "Новая задача добавлена: " + goal.Name,
                    DateCreate = DateTime.Now,
                    UserId = user.Id
                });

                await db.SaveChangesAsync();

                ViewBag.Message = "Задача успешно добавлена.";
                return PartialView("_Success");
            }

            List<SelectListItem> skills = new SelectList(db.Skills.Where(s => s.UserId == usermanager.GetUserId(User)), "SkillId", "Name").ToList();
            skills.Insert(0, new SelectListItem() { Text = "Нет", Value = "0" });

            ViewBag.RepeatId = new SelectList(await db.Repeats.ToListAsync(), "RepeatId", "Name",goal.RepeatId);
            ViewBag.ComplicationId = new SelectList(await db.Complications.ToListAsync(), "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.CatalogId = new SelectList(await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).ToListAsync(), "CatalogId", "Name",goal.CatalogId);
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
            ViewBag.CatalogId = new SelectList(await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).ToListAsync(), "CatalogId", "Name", goal.CatalogId);
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

            ViewBag.RepeatId = new SelectList(db.Repeats, "RepeatId", "Name",goal.RepeatId);
            ViewBag.ComplicationId = new SelectList(db.Complications, "ComplicationId", "Name", goal.ComplicationId);
            ViewBag.CatalogId = new SelectList(await db.Catalogs.Where(c => c.UserId == usermanager.GetUserId(User)).ToListAsync(), "CatalogId", "Name",goal.CatalogId);
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

            await db.Notifications.AddAsync(new Notification()
            {
                Name = "Задача удалена: " + goal.Name,
                DateCreate = DateTime.Now,
                UserId = goal.UserId
            });

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
                goal.Skill.LvlUp += AddNotification;
                goal.Skill.RatingUp += AddNotification;

                goal.Skill.ExpUp();
            }

            goal.User.CheckLvl();

            await db.Notifications.AddAsync(new Notification()
            {
                Name = "Задача выполнена: " + goal.Name,
                DateCreate = DateTime.Now,
                UserId = goal.UserId
            });

            await db.SaveChangesAsync();

            ViewBag.Message = "Задача выполнена - " +  goal.Name;
            return PartialView("_Success");
        }


        public async Task<IActionResult> RestoreGoal(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Goal goal = await db.Goals.Where(g => g.GoalId == id)
                .Include(g => g.User)
                .Include(g => g.Skill)
                .Include(g => g.Skill.Atribute)
                .Include(g => g.Complication)
                .Include(g => g.Repeat)
                .SingleOrDefaultAsync();

            if (goal == null)
            {
                return NotFound();
            }

            goal.IsComplete = false;

            await db.SaveChangesAsync();

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
                    RedirectToAction("Index", "Home");
                }

                catalog.UserId = user.Id;
                await db.Catalogs.AddAsync(catalog);

                await db.Notifications.AddAsync(new Notification()
                {
                    Name = "Каталог добавлен: " + catalog.Name,
                    DateCreate = DateTime.Now,
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
                DateCreate = DateTime.Now,
                UserId = catalog.UserId
            });

            await db.SaveChangesAsync();
            return Ok();
        }

        private void AddNotification(object sender, NotificationEventArgs e)
        {

            db.Notifications.Add(new Notification()
            {
                Name = e.Name,
                DateCreate = DateTime.Now,
                UserId = e.UserId
            });

            db.SaveChanges();
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