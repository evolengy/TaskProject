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

            //if (!user.IsSetDescr)
            //{
            //    ViewSetProfileModel view = new ViewSetProfileModel();

            //    List<Atribute> DefaultAtributes = db.Atributes.ToList();

            //    foreach (var DefaultAtribute in DefaultAtributes)
            //    {
            //            view.UserAtributes.Add(new UserAtribute()
            //            {
            //                Atribute = DefaultAtribute,
            //                AtributeId = DefaultAtribute.AtributeId,                          
            //            });
            //    }
            //    return View("SetProfile", view);
            //}

            CheckGoals(user);

            if (user.IsDead || user.CurrentHealth <= 0)
            {
                user.CurrentHealth = 0;
                user.IsDead = true;
                db.SaveChanges();
            }
            db.SaveChanges();

            return View(user);
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

        private void CheckGoals(ApplicationUser user)
        {
            foreach (var goal in user.Goals)
            {
                if (goal.TaskEnd < DateTime.Now && goal.IsComplete == false && (goal.TaskEnd.Value.Day - goal.TaskStart.Day) > TimeSpan.FromDays(1).Days)
                {
                    do
                    {
                        user.CurrentHealth = user.CurrentHealth - goal.Complication.Damage;
                        switch (goal.RepeatId)
                        {
                            case 2:
                                {
                                    goal.TaskEnd = null;
                                    break;
                                }
                            case 3:
                                {
                                    goal.TaskEnd = goal.TaskEnd.Value.AddDays(1);
                                    break;
                                }
                            case 5:
                                {
                                    goal.TaskEnd = goal.TaskEnd.Value.AddMonths(1);
                                    break;
                                }
                            case 6:
                                {
                                    goal.TaskEnd = goal.TaskEnd.Value.AddYears(1);
                                    break;
                                }
                        }
                    }
                    while (goal.TaskEnd < DateTime.Now);
                    db.SaveChanges();
                }
            }
        }
    }
}
