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
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public NotificationsController(ApplicationDbContext _db, UserManager<ApplicationUser> _usermanager)
        {
            db = _db;
            usermanager = _usermanager;
        }

        public async Task<IActionResult> DeleteAllNotifications()
        {
            var user = await usermanager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            var notifications = await db.Notifications.Where(n => n.UserId == user.Id).ToListAsync();

            db.Notifications.RemoveRange(notifications);

            await db.SaveChangesAsync();

            return RedirectToAction("GameRoom", "Home");
        }

    }
}
