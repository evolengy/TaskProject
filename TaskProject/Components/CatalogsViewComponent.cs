using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskProject.Models;
using TaskProject.Models.GoalModels;

namespace TaskProject.Components
{
    public class Catalogs : ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public Catalogs(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            usermanager = userManager;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var userid = usermanager.GetUserId(UserClaimsPrincipal);
            List<Catalog> catalogs = await db.Catalogs.Where(c => c.UserId == userid).ToListAsync();
            return View("GetCatalogs",catalogs);
        }
    }
}
