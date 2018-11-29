using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Linq;
using System.Threading.Tasks;
using TaskProject.Models;

namespace TaskProject.Components
{
    public class Profile: ViewComponent
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public Profile(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            usermanager = userManager;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            string userId = usermanager.GetUserId(UserClaimsPrincipal);

            var user = db.Users.Single(u => u.Id == userId);
            
            return View("GetProfile", user);
        }
    }
}
