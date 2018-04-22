using System;
using System.Collections.Generic;
using System.Linq;
using TaskLibrary;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Spacewander.Controllers
{
    public class AchievmentsController : Controller
    {

        private ApplicationDbContext db;

        public ActionResult Index(ApplicationDbContext _db)
        {
            db = _db;
            List<Attainment> GlobalAttainments = db.Attainments.ToList();

            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
            {
                return new StatusCodeResult(400);
            }
            int id = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());
            Character Character = db.Characters.Where(c => c.CharacterId == id).SingleOrDefault();

            foreach(Attainment GlobalAttainment in GlobalAttainments)
            {
                if (!Character.Attainments.Contains(GlobalAttainment))
                {
                    GlobalAttainment.LinkImage = "/img/attainment/secret.png";
                }
            }

            return View(GlobalAttainments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}