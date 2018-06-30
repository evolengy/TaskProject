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
    public class AchievementsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public AchievementsController(ApplicationDbContext _db, UserManager<ApplicationUser> _usermanager)
        {
            db = _db;
            usermanager = _usermanager;
        }

        public async Task<IActionResult> GetAchievements()
        {
            var user = await usermanager.GetUserAsync(User);

            if(user == null)
            {
                RedirectToAction("Index", "Home");
            }

            var dbUser = await db.Users.Where(u => u.Id == user.Id).Include(u => u.UserAchievements).SingleOrDefaultAsync();

            var dbAchievements = await db.Achievements.ToListAsync();

            foreach(var achievement in dbAchievements)
            {
                if(!dbUser.UserAchievements.Exists(a => a.AchievementId == achievement.AchievementId))
                {
                    user.UserAchievements.Add(new UserAchievement()
                    {
                        Achievement = achievement
                    });
                }
            }
            await db.SaveChangesAsync();

            ViewBag.BreadCrumb = "Достижения";

            return View(dbUser.UserAchievements);
        }
    }
}
