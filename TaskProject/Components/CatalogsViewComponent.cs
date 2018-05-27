using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskProject.Models;

namespace TaskProject.Components
{
    public class Catalogs : ViewComponent
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public Catalogs(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            usermanager = _userManager;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var userid = usermanager.GetUserId(UserClaimsPrincipal);
            List<Catalog> catalogs = await db.Catalogs.Where(c => c.UserId == userid).ToListAsync();
            return View("GetCatalogs",catalogs);
        }
    }
}
