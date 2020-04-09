using TaskProject.Models;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TaskProject.Services.EmailSender;

namespace TaskProject.Controllers
{
	public class InfoController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IConfiguration configuration;

        public InfoController(ApplicationDbContext _db, IConfiguration _configuration)
        {
            db = _db;
            configuration = _configuration;
        }

        public ActionResult FAQ()
        {
            Message message = new Message();

            ViewBag.BreadCrumb = "Руководство";
            return View(message);
        }

        public ActionResult Report()
        {
            ViewBag.BreadCrumb = "Сообщить об ошибке";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Report(Message model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BreadCrumb = "Сообщить об ошибке";
                return View(model);
            }

            model.Theme = "Report";
            model.DateCreate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString(CultureInfo.CurrentCulture);

            EmailSender sender = new EmailSender(configuration);
            await sender.SendEmailToOwnerAsync(model.Email, model.Theme, model.Body);

            await db.Messages.AddAsync(model);

            await db.SaveChangesAsync();
            return RedirectToAction("GameRoom","Home");
        }

        public ActionResult QuestionAndSuggestion()
        {
            ViewBag.BreadCrumb = "Вопросы и предложения";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> QuestionAndSuggestion(Message model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BreadCrumb = "Вопросы и предложения";
                return View(model);
            }
            model.Theme = "QuestionAndSuggestion";
            model.DateCreate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString(CultureInfo.CurrentCulture);

            EmailSender sender = new EmailSender(configuration);
            await sender.SendEmailToOwnerAsync(model.Email, model.Theme, model.Body);

            await db.Messages.AddAsync(model);
            await db.SaveChangesAsync();
            return RedirectToAction("GameRoom", "Home");
        }
    }
}