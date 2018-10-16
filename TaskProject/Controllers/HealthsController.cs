using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class HealthsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> usermanager;

        public HealthsController(ApplicationDbContext _db, UserManager<ApplicationUser> _usermanager)
        {
            db = _db;
            usermanager = _usermanager;
        }

        public async Task<ActionResult> Index()
        {
            var user = await usermanager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            Health health = await db.Healths.Where(h => h.UserId == user.Id).Include(h => h.UserListGrowth).Include(h => h.UserListWeight).SingleOrDefaultAsync();

            //Get Last Growth and Weight
            if (health.UserListGrowth.Any() && health.UserListWeight.Any())
            {
                ViewBag.Growth = health.UserListGrowth.Last().Value;
                ViewBag.Weight = health.UserListWeight.Last().Value;

                ViewBag.IMT = health.GetIMT(ViewBag.Growth, ViewBag.Weight);
                ViewBag.IMTGroup = health.GetAgeGroup();
            }

            ViewBag.BreadCrumb = "Здоровье";
            return View(health);
        }

        public async Task<IActionResult> SetProfile()
        {
            var user = await usermanager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            ViewSetProfileModel view = new ViewSetProfileModel();
            ViewBag.BreadCrumb = "Анкета";
            return View("SetProfile", view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetProfile(ViewSetProfileModel view)
        {
            if (ModelState.IsValid)
            {
                var user = await usermanager.GetUserAsync(User);
                if (user == null)
                {
                    return View("Index");
                }

                Health userhealth = await db.Healths.Where(h => h.UserId == user.Id).SingleOrDefaultAsync();

                db.Entry(userhealth).State = EntityState.Modified;

                userhealth.Sex = view.Sex;
                userhealth.DateBirth = view.DateBirth;

                userhealth.UserListGrowth.Add(new UserGrowth()
                {
                    Value = (float)view.Growth,
                    Date = DateTime.Now,
                    HealthId = userhealth.HealthId
                });

                userhealth.UserListWeight.Add(new UserWeight()
                {
                    Value = (float)view.Weight,
                    Date = DateTime.Now,
                    HealthId = userhealth.HealthId
                });

                if (view.Sex == "Man")
                {
                    view.DateDeath = view.DateBirth.Value.AddYears(60);
                }
                else
                {
                    view.DateDeath = view.DateBirth.Value.AddYears(72);
                }

                IMT imt = userhealth.GetIMT(view.Growth,view.Weight);

                if (imt.Class == "Норма")
                {
                    view.DateDeath = view.DateDeath.AddDays(+657);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-657);
                }

                if (view.IsSmoke)
                {
                    view.DateDeath = view.DateDeath.AddYears(-2);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddYears(+2);
                }

                if (view.IsFastFood)
                {
                    view.DateDeath = view.DateDeath.AddDays(-219);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(+219);
                }

                if (view.IsFriedFood)
                {
                    view.DateDeath = view.DateDeath.AddDays(-146);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(+146);
                }

                if (view.IsFattyFood)
                {
                    view.DateDeath = view.DateDeath.AddYears(-2);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddYears(+2);
                }

                if (view.Food == "green")
                {
                    view.DateDeath = view.DateDeath.AddDays(+657);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-657);
                }

                if (view.IsAlcoholDrink)
                {
                    view.DateDeath.AddDays(-438);
                }
                else
                {
                    view.DateDeath.AddDays(+219);
                }

                if (view.IsAdversePlace)
                {
                    view.DateDeath = view.DateDeath.AddYears(-1);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddYears(+1);
                }

                if (view.IsCoffee)
                {
                    view.DateDeath = view.DateDeath.AddDays(-219);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(+219);
                }

                if (view.IsAspirin)
                {
                    view.DateDeath = view.DateDeath.AddDays(+292);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-292);
                }

                if (view.IsDentalFloss)
                {
                    view.DateDeath = view.DateDeath.AddDays(+438);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-438);
                }

                if (view.IsRegularChair)
                {
                    view.DateDeath = view.DateDeath.AddDays(+292);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-292);
                }

                if (view.IsSexRelation)
                {
                    view.DateDeath = view.DateDeath.AddDays(-584);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(+584);
                }

                if (view.IsStrongTan)
                {
                    view.DateDeath = view.DateDeath.AddDays(-511);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(+511);
                }

                if (view.IsMarriage)
                {
                    view.DateDeath = view.DateDeath.AddDays(-657);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(+657);
                }

                if (view.IsStress)
                {
                    view.DateDeath = view.DateDeath.AddDays(+511);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-511);
                }

                if (view.IsDiabetes)
                {
                    view.DateDeath = view.DateDeath.AddDays(-292);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(+292);
                }

                if (view.IsParents75)
                {
                    view.DateDeath = view.DateDeath.AddYears(-2);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddYears(+2);
                }

                if (view.IsParents90)
                {
                    view.DateDeath = view.DateDeath.AddDays(+1752);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-1752);
                }

                if (view.IsSport)
                {
                    view.DateDeath = view.DateDeath.AddDays(+511);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-511);
                }

                if (view.IsVitamit)
                {
                    view.DateDeath = view.DateDeath.AddDays(+584);
                }
                else
                {
                    view.DateDeath = view.DateDeath.AddDays(-584);
                }

                userhealth.DateDeath = view.DateDeath;
                userhealth.IsSetValue = true;

                user.Notifications.Add(new Notification()
                {
                    Name = "Пользователь заполнил анкету",
                    DateCreate = DateTime.Now
                });

                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Healths");

            }

            ViewBag.BreadCrumb = "Анкета";
            return View(view);
        }


        [HttpPost]
        public async Task<ActionResult> ChangeParam(float growth, float weight)
        {
            if (growth == 0 || weight == 0)
            {
                return RedirectToAction("Index");
            }

            var user = await usermanager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }


            var health = await db.Healths.Where(h => h.UserId == user.Id).SingleOrDefaultAsync();

            db.Entry(health).State = EntityState.Modified;

            health.UserListGrowth.Add(new UserGrowth()
            {
                Value = growth,
                Date = DateTime.Now
            });

            health.UserListWeight.Add(new UserWeight()
            {
                Value = weight,
                Date = DateTime.Now
            });

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
