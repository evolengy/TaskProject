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
    public class MoodsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public MoodsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;

        }

        [HttpPost]
        public async Task<IActionResult> AddTodayMood(Mood mood)
        {
            if (mood.MoodId == 0)
            {
                return RedirectToAction("GameRoom", "Home");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.GetUserAsync(User);

                user.UserMoods.Add(new UserMood() {
                    MoodId = mood.MoodId
                });

                db.Notifications.Add(new Notification()
                {
                    Name = "Отмечено настроение за день",
                    DateCreate = DateTime.Now,
                    UserId = user.Id
                });

                await db.SaveChangesAsync();
            }

            return RedirectToAction("GameRoom", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTodayMood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await userManager.GetUserAsync(User);

            UserMood todaymood = await db.UserMoods.Where(m => m.UserMoodId == id).SingleOrDefaultAsync();

            if (todaymood == null)
            {
                return NotFound();
            }

            db.UserMoods.Remove(todaymood);

            db.Notifications.Add(new Notification()
            {
                Name = "Удалено настроение за день",
                DateCreate = DateTime.Now,
                UserId = user.Id
            });

            await db.SaveChangesAsync();

            return RedirectToAction("GameRoom", "Home");
        }

        public IActionResult StatMoods()
        {
            ViewBag.BreadCrumb = "Статистика хороших и плохих дней";
            return View();
        }


        public async Task<IActionResult> GetMonthMood()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var month = DateTime.Now.Month;
            var moods = await db.UserMoods
                .Where(m => m.UserId == user.Id && m.Date.Month == month).Include(m => m.Mood)
                .GroupBy(m => m.Mood.Name)
                .Select(g => new {Name = g.Key, Count = g.Count() })
                .ToListAsync();

            List<string> count = new List<string>();
            List<string> name = new List<string>();
            List<string> color = new List<string>();

            List<List<string>> all = new List<List<string>>();

            foreach( var mood in moods)
            {
                name.Add(mood.Name);
                count.Add(mood.Count.ToString());

                switch (mood.Name)
                {
                    case "Плохое":
                        {
                            color.Add("rgb(219, 20, 20)");
                            break;
                        }
                    case "Сонное":
                        {
                            color.Add("rgb(6, 135, 199)");
                            break;
                        }
                    case "Нормальное":
                        {
                            color.Add("rgb(60, 139, 230)");
                            break;
                        }
                    case "Веселое":
                        {
                            color.Add("rgb(19, 255, 15)");
                            break;
                        }
                    case "Отличное":
                        {
                            color.Add("rgb(74, 150, 15)");
                            break;
                        }
                }
            }

            all.Add(name);
            all.Add(count);
            all.Add(color);
            return Json(all);
        }
    }
}
