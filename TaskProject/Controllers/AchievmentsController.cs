using System;
using System.Collections.Generic;
using System.Linq;
using TaskProject.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TaskLibrary;

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
            if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
            {
                return new StatusCodeResult(400);
            }
            string id = HttpContext.Request.Cookies["UserId"];
            ApplicationUser User = db.Users.Where(c => c.Id == id).SingleOrDefault();

            foreach(Attainment GlobalAttainment in GlobalAttainments)
            {
                if (!User.Attainments.Contains(GlobalAttainment))
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