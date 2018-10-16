using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class SkillsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public SkillsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            usermanager = _userManager;
        }

        public IActionResult AddSkill()
        {
            ViewData["AtributeId"] = new SelectList(db.Atributes.Where(s => s.UserId == usermanager.GetUserId(User)), "AtributeId", "Name");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSkill([Bind("SkillId,Name,Lvl,CurrentExp,MaxExp,UserId,AtributeId,RatingId")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);
                if (user == null)
                {
                    RedirectToAction("Logout", "Account");
                }

                skill.UserId = user.Id;

                await db.AddAsync(skill);

                await db.Notifications.AddAsync(new Notification()
                {
                    Name = "Добавлен новый навык: " + skill.Name,
                    DateCreate = DateTime.Now,
                    UserId = user.Id
                });

                await db.SaveChangesAsync();

                ViewBag.Message = "Навык успешно добавлен.";
                return PartialView("_Success");
            }
            ViewData["AtributeId"] = new SelectList(db.Atributes.Where(s => s.UserId == usermanager.GetUserId(User)), "AtributeId", "Name",skill.SkillId);
            return PartialView(skill);
        }

        public async Task<IActionResult> EditSkill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skill = await db.Skills.SingleOrDefaultAsync(m => m.SkillId == id);
            if (skill == null)
            {
                return NotFound();
            }
            ViewData["AtributeId"] = new SelectList(db.Atributes.Where(s => s.UserId == usermanager.GetUserId(User)), "AtributeId", "Name",skill.SkillId);
            return View(skill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSkill(Skill skill)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(skill);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(skill.SkillId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Message = "Навык успешно изменен.";
                return PartialView("_Success");
            }
            ViewData["AtributeId"] = new SelectList(db.Atributes.Where(s => s.UserId == usermanager.GetUserId(User)), "AtributeId", "Name",skill.SkillId);
            return View(skill);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skill = await db.Skills.Include(s => s.Goals).SingleOrDefaultAsync(m => m.SkillId == id);

            if (skill == null)
            {
                return NotFound();
            }

            db.Notifications.Add(new Notification()
            {
                Name = "Удален навык: " + skill.Name,
                DateCreate = DateTime.Now,
                UserId = skill.UserId
            });

            db.Skills.Remove(skill);
            await db.SaveChangesAsync();
            return Ok();
        }

        private bool SkillExists(int id)
        {
            return db.Skills.Any(e => e.SkillId == id);
        }
    }
}
