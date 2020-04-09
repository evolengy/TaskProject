using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;
using TaskProject.Models.RatingViewModels;

namespace TaskProject.Controllers
{
	public class UsersController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public UsersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            usermanager = userManager;
        }

        public IActionResult GetKarmaForUsers()
        {
            List<ApplicationUser> users = db.Users.Include(u => u.Karma).ToList();
            List<KarmaViewModel> model = new List<KarmaViewModel>();

            foreach (var user in users)
            {
                model.Add(new KarmaViewModel(user.Email, user.Karma.Count, user.Karma.Count(k => k.IsGood)));
            }

            var karmaViewModels = model.OrderBy(m => m.AllKarma);

            ViewBag.BreadCrumb = "Рейтинг кармы";

            return View(karmaViewModels);
        }

        public IActionResult GetGoalsForUsers()
        {
            List<ApplicationUser> users = db.Users.Include(u => u.Goals).ToList();
            List<GoalViewModel> model = new List<GoalViewModel>();

            foreach (var user in users)
            {
                model.Add(new GoalViewModel(user.Email, user.Goals.Count, user.Goals.Count(g => g.IsComplete)));
            }

            var goalViewModels = model.OrderBy(m => m.AllGoals);

            ViewBag.BreadCrumb = "Рейтинг задач";

            return View(goalViewModels);
        }
    }
}