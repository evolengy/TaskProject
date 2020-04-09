using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;
using TaskProject.Models.AccountViewModels;

namespace TaskProject.Controllers
{
	public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public HomeController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            usermanager = _userManager;
        }

        public ActionResult Index()
        {
            if (User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("GameRoom");
                }
            }
            return View();
        }

        public async Task<ActionResult> GameRoom()
        {

            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            // Значения по умолчанию
            if (!user.IsSetDefaultValues)
            {
                user.SetDefaultValues();
                await db.SaveChangesAsync();
            }

            user = await db.GetUser(user.Id);
            await db.CheckGoal(user);

            var allmood = await db.Moods.ToListAsync();
            var todaymood = user.GetTodayMood();

            if (todaymood == null)
            {
                ViewBag.TodayMood = null;
                ViewBag.ListMood = allmood;
            }
            else
            {
                ViewBag.TodayMood = todaymood;
            }

            Note todaynote = user.Notes.SingleOrDefault(n => n.DateCreate.Value.Date == TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Date);
            if (todaynote == null)
            {
                ViewBag.TodayNote = null;
            }
            else
            {
                ViewBag.TodayNote = todaynote;
            }  

            await db.SaveChangesAsync();

            ViewBag.BreadCrumb = "Персонаж";
            return View(user);
        }

        public ActionResult AddNickName()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNickName(NickNameViewModel model)
        {
            if (db.Users.Any(u => u.NickName == model.NickName))
            {
                ModelState.AddModelError("NickName", "Пользователь с данным ником уже существует");
            }

            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);

                if (user == null)
                {
                    RedirectToAction("Logout", "Account");
                }

                user.NickName = model.NickName;
                await db.SaveChangesAsync();

                return PartialView("_Success");
            }

            return View(model);
        }

        public async Task<IActionResult> EditNickName()
        {
            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            NickNameViewModel model = new NickNameViewModel();
            model.NickName = user.NickName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNickName(NickNameViewModel model)
        {
            if (db.Users.Any(u => u.NickName == model.NickName))
            {
                ModelState.AddModelError("NickName", "Пользователь с данным ником уже существует");
            }

            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);

                if (user == null)
                {
                    RedirectToAction("Logout", "Account");
                }
                user.NickName = model.NickName;
                await db.SaveChangesAsync();

                return PartialView("_Success");
            }
            return View(model);
        }

        public ActionResult Error(string id = null)
        {
            if(id == "403")
            {
                ViewBag.Code = "Ошибка 403.";
                ViewBag.Title = "Отказано в доступе.";
                ViewBag.Message = "В доступе отказано. Пожалуйста вернитесь назад.";
            }
            if(id == "404")
            {
                ViewBag.Code = "Ошибка 404.";
                ViewBag.Title = "Упс! Страница не найдена.";
                ViewBag.Message = "Страница, которую вы ищете не может быть найдена.";
            }
            else
            {
                ViewBag.Code = "Ошибка 500.";
                ViewBag.Title = "Внутренняя ошибка сервера.";
                ViewBag.Message = "Похоже мы испытываем проблемы. Но не волнуйтесь, скоро мы их разрешим. Пожалуйста, попробуйте попозже.";
            }
            return View();
        }  
    }
}
