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
            if(ModelState.IsValid)
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                user.Moods.Add(mood);

                await db.SaveChangesAsync();
            }

            return RedirectToAction("GameRoom","Home");
        }

        public async Task<IActionResult> GetMoods()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Mood> moods = await db.Moods.Where(m => m.UserId == user.Id).ToListAsync();
            return View();
        }
    }    
}
