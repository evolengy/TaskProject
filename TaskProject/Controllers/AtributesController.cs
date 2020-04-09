using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;

namespace TaskProject.Controllers
{
	public class AtributesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public AtributesController(ApplicationDbContext _db, UserManager<ApplicationUser> _usermanager)
        {
            db = _db;
            usermanager = _usermanager;
        }

        public async Task<IActionResult> GetAtributes()
        {
            var user = await usermanager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            var atributes = await db.Atributes.Where(a => a.UserId == user.Id).Include(a => a.Skills).ToListAsync();

            ViewBag.BreadCrumb = "Характеристики";

            return View(atributes);
        }

    }
}
