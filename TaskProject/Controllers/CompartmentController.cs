using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
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
            if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
            {
                return new StatusCodeResult(400);
            }

            int id = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());

            Character Character = db.Characters.Where(c => c.CharacterId == id).FirstOrDefault();
            return View(Character);
        }

        public ActionResult GetCare()
        {
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
            {
                return new StatusCodeResult(400);
            }

            int id = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());

            Character Character = db.Characters.Where(c => c.CharacterId == id).FirstOrDefault();

            Character.CurrentGold = Character.CurrentGold - (Character.MaxHealth - Character.CurrentHealth)*2;
            Character.CurrentHealth = Character.MaxHealth;
            if (Character.IsDead)
            {
                Character.IsDead = false;
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