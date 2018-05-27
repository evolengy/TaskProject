using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject;
using TaskProject.Models;

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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("GameRoom");
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

            if (user.IsSetValue == false)
            {
                return RedirectToAction("SetProfile");
            }

            var dbuser = await db.Users.Where(u => u.Id == user.Id)
                .Include(u => u.Goals)
                .Include(u => u.Skills)
                .Include(u => u.Atributes)
                .Include(u => u.Aligment)
                .Include(u => u.Moods)
                .Include(u => u.Notes)
                .SingleAsync();

            dbuser.RefreshStatus();

            Mood todaymood = dbuser.Moods.Where(m => m.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).SingleOrDefault();
            if(todaymood == null)
            {
                ViewBag.TodayMood = null;
                ViewBag.ListMood = Mood.GetMoods();
            }
            else
            {
                todaymood.GetLink();
                ViewBag.TodayMood = todaymood;
            }

            Note todaynote = dbuser.Notes.Where(n => n.DateCreate.ToShortDateString() == DateTime.Now.ToShortDateString()).SingleOrDefault();
            if(todaynote == null)
            {
                ViewBag.TodayNote = null;
            }
            else
            {
                ViewBag.TodayNote = todaynote;
            }

            await db.SaveChangesAsync();


            ViewBag.BreadCrumb = "Управление персонажем";
            return View(dbuser);
        }

        public async Task<IActionResult> SetProfile()
        {
            var user = await usermanager.GetUserAsync(User);
            if (user == null || user.IsSetValue == true)
            {
                return View("GameRoom");
            }

            ViewSetProfileModel view = new ViewSetProfileModel();
            view.AligmentSelect = await db.Aligments.ToListAsync();

            ViewBag.SetProfile = false;
            return View("SetProfile", view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetProfile(ViewSetProfileModel view)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);
                if (user == null)
                {
                    return View("Index");
                }

                db.Entry(user).State = EntityState.Modified;

                user.DateBirth = view.DateBirth;
                user.Weight = view.Weight;
                user.Growth = view.Growth;
                user.Sex = view.Sex;
                user.AligmentId = view.AligmentId;

                user.SetDefaultValues();
                user.RefreshStatus();

                if(user.Health.IMT.Class == "Норма")
                {
                    var count = "1.8";
                }

                user.IsSetValue = true;
                await db.SaveChangesAsync();

                return RedirectToAction("GameRoom", "Home");

            }
            return View(view);
        }


        public async Task<ActionResult> Health()
        {
            var user = await usermanager.GetUserAsync(User);
            if(user == null)
            {
                return View("GameRoom");
            }
        
            user.RefreshStatus();
            await db.SaveChangesAsync();
            return View(user.Health);
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

        public ActionResult Dead()
        {
            return PartialView();
        }
    }
}
