using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                return View("Index");
            }

            if (user.IsSetValue == false)
            {
                user.SetDefaultValue();
                await db.SaveChangesAsync();

                //    ViewSetProfileModel view = new ViewSetProfileModel();
                //    return View("SetProfile", view);
            }

            var dbuser = db.Users.Where(u => u.Id == user.Id)
                .Include(u => u.Goals)
                .Include(u => u.Skills)
                .Include(u => u.Atributes)
                .Include(u => u.Aligment)
                .Include(u => u.Moods)
                .Include(u => u.Notes)
                .Single();

            dbuser.CheckStatus();

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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SetProfile(ViewSetProfileModel view)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await usermanager.GetUserAsync(User);
        //        if (user == null)
        //        {
        //            return View("Index");
        //        }

        //        foreach (UserAtribute atribute in view.UserAtributes)
        //        {
        //            atribute.MaxExp = atribute.Value * 1000;
        //        }

        //        db.Entry(user).State = EntityState.Modified;
        //        user.Age = view.Age;
        //        user.Weight = view.Weight;
        //        user.Growth = view.Growth;
        //        user.Sex = view.Sex;
        //        db.SaveChanges();

        //        return View("GameRoom","Home");

        //    }
        //    return View(view);
        //}

        public ActionResult Dead()
        {
            return PartialView();
        }
    }
}
