using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    //Todo Добавить возможность получать подарки за получение кармы
    public class KarmaController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public KarmaController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            usermanager = userManager;
        }

        public async Task<ActionResult> GetKarma()
        {
            var user = await usermanager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            List<Karma> karma = await db.Karma.Where(k => k.UserId == user.Id).ToListAsync();
            ViewBag.BreadCrumb = "Все поступки";

            return View(karma);
        }

        // GET: Karma/Create
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Karma karma)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                karma.UserId = user.Id;
                await db.AddKarmaAsync(karma);

                ViewBag.Message = "Задача успешно добавлена.";
                return PartialView("_Success");
            }

            return PartialView(karma);
        }
    }
}