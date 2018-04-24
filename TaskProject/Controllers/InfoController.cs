using TaskProject.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using TaskProject.Services;
using TaskLibrary;

namespace Spacewander.Controllers
{
    public class InfoController : Controller
    {
        private ApplicationDbContext db;


        public ActionResult FAQ(ApplicationDbContext _db)
        {
            db = _db;
            Message message = new Message();
            return View(message);
        }

        public ActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Report(Message model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Theme = "Report";
            model.DateCreate = DateTime.Now.ToString();

            EmailSender sender = new EmailSender();
            sender.SendEmailAsync(model.Email, model.Theme, model.Body);

            db.Messages.Add(model);
            db.SaveChanges();
            return RedirectToAction("GameRoom","Home");
        }

        public ActionResult QuestionAndSuggestion(ApplicationDbContext _db)
        {
            db = _db;
            return View();
        }

        [HttpPost]
        public ActionResult QuestionAndSuggestion(Message model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Theme = "QuestionAndSuggestion";
            model.DateCreate = DateTime.Now.ToString();

            EmailSender sender = new EmailSender();
            sender.SendEmailAsync(model.Email, model.Theme, model.Body);

            db.Messages.Add(model);
            db.SaveChanges();
            return RedirectToAction("GameRoom", "Home");
        }
    }
}