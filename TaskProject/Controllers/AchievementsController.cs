using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;
using TaskProject.Models.AchievementModels;

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
                RedirectToAction("Logout", "Account");
            }

            var dbUser = await db.Users.Where(u => u.Id == user.Id).Include(u => u.UserAchievements).SingleOrDefaultAsync();
            await db.GetAchievementsAsync(dbUser);

            ViewBag.BreadCrumb = "Достижения";

            return View(dbUser.UserAchievements);
        }
    }
}
