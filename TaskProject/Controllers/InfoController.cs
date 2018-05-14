using TaskProject.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using TaskProject.Services;
using TaskProject;

namespace TaskProject.Controllers
{
    public class InfoController : Controller
    {
        private readonly ApplicationDbContext db;

        public InfoController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public ActionResult FAQ()
        {
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

        public ActionResult QuestionAndSuggestion()
        {
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