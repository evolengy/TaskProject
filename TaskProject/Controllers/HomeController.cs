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

            // Значения по умолчанию
            if (!user.IsSetDefaultValues)
            {
                user.SetDefaultValues();
                await db.SaveChangesAsync();
            }

            var dbuser = await db.Users.Where(u => u.Id == user.Id)
                .Include(u => u.Goals)
                .Include(u => u.Skills)
                .Include(u => u.Atributes)
                .Include(u => u.Aligment)
                .Include(u => u.UserMoods)
                .Include(u => u.Notes)
                .SingleAsync();

            var allmood = await db.Moods.ToListAsync();
            var todaymood = await db.UserMoods.Where(m => m.UserId == dbuser.Id && m.Date.ToShortDateString() == DateTime.Now.ToShortDateString()).Include(m => m.Mood).SingleOrDefaultAsync();

            if (todaymood == null)
            {
                ViewBag.TodayMood = null;
                ViewBag.ListMood = allmood;
            }
            else
            {
                ViewBag.TodayMood = todaymood;
            }

            Note todaynote = dbuser.Notes.Where(n => n.DateCreate.ToShortDateString() == DateTime.Now.ToShortDateString()).SingleOrDefault();
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
            return View(dbuser);
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
