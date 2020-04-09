using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskProject.Models;

namespace TaskProject.Components
{
	public class Notifications : ViewComponent
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public Notifications(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            usermanager = _userManager;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var userid = usermanager.GetUserId(UserClaimsPrincipal);
            List<Notification> notifications = await db.Notifications.Where(c => c.UserId == userid).OrderByDescending(c => c.DateCreate).ToListAsync();
            return View("GetNotifications", notifications);
        }
    }
}
