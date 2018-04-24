using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using TaskProject.Models;
using TaskLibrary;

namespace Spacewander.Controllers
{
    public class CompartmentController : Controller
    {

        ApplicationDbContext db;

        public ActionResult Medical(ApplicationDbContext _db)
        {
            db = _db;
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
            {
                return new StatusCodeResult(400);
            }

            string id = HttpContext.Request.Cookies["UserId"];

            ApplicationUser User = db.Users.Where(c => c.Id == id).FirstOrDefault();
            return View(User);
        }

        public ActionResult GetCare()
        {
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
            {
                return new StatusCodeResult(400);
            }

            string id = HttpContext.Request.Cookies["UserId"];

            ApplicationUser User = db.Users.Where(c => c.Id == id).FirstOrDefault();

            User.CurrentGold = User.CurrentGold - (User.MaxHealth - User.CurrentHealth)*2;
            User.CurrentHealth = User.MaxHealth;
            if (User.IsDead)
            {
                User.IsDead = false;
            }

            db.SaveChanges();
            return RedirectToAction("Medical");
        }

        public ActionResult Technical(ApplicationDbContext _db)
        {
            db = _db;
            return View();
        }
    }
}