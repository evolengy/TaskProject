using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Models;
using ChartJSCore.Models.Bar;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;
using TaskProject.Models.DateTimeModels;

namespace TaskProject.Controllers
{
  
    public class MoodsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public MoodsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddTodayMood(Mood mood)
        {
            if (mood.MoodId == 0)
            {
                return RedirectToAction("GameRoom", "Home");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                user.UserMoods.Add(new UserMood()
                {
                    MoodId = mood.MoodId
                });

                db.Notifications.Add(new Notification()
                {
                    Name = "Отмечено настроение за день",
                    DateCreate = DateTime.Now,
                    UserId = user.Id
                });

                await db.SaveChangesAsync();
            }

            return RedirectToAction("GameRoom", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTodayMood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await userManager.GetUserAsync(User);

            UserMood todaymood = await db.UserMoods.Where(m => m.UserMoodId == id).SingleOrDefaultAsync();

            if (todaymood == null)
            {
                return NotFound();
            }

            db.UserMoods.Remove(todaymood);

            db.Notifications.Add(new Notification()
            {
                Name = "Удалено настроение за день",
                DateCreate = DateTime.Now,
                UserId = user.Id
            });

            await db.SaveChangesAsync();

            return RedirectToAction("GameRoom", "Home");
        }

        public IActionResult StatMoods(int month)
        {
            Year year = new Year();
            Month selectmonth;

            if (month == 0)
            {
                selectmonth = year.Months.Single(m => m.MonthId == DateTime.Now.Month);
            }
            else
            {
                selectmonth = year.Months.Single(m => m.MonthId == month);
            }

            ViewBag.BreadCrumb = "Статистика хороших и плохих дней";

           
            ViewBag.Month = new SelectList(year.Months, "MonthId", "MonthName",  selectmonth.MonthId);
            
            ViewBag.MonthChart = GetMonthMood(selectmonth);
            ViewBag.YearChart = GetYearMood();
            
            return View();
        }

        public Chart GetMonthMood(Month selectedMonth)
        {
            Chart chart = new Chart();
            chart.Type = "pie";

            Data data = new Data();
            data.Labels = new List<string>() { };

            PieDataset dataset = new PieDataset()
            {
                Label = "Статистика за " + selectedMonth.MonthName,
                BackgroundColor = new List<string>(),
                HoverBackgroundColor = new List<string>(),
                Data = new List<double>()
            };

            var userId = userManager.GetUserId(User);
            var moods = db.UserMoods
                .Where(m => m.UserId == userId && m.Date.Month == selectedMonth.MonthId).Include(m => m.Mood)
                .GroupBy(m => m.Mood.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToList();

            if (!moods.Any())
            {
                data.Labels.Add("Нет записей");
                dataset.Data.Add(1);
                dataset.BackgroundColor.Add("rgb(169,169,169)");
            }
            else
            {
                foreach (var mood in moods)
                {
                    data.Labels.Add(mood.Name);
                    dataset.Data.Add(mood.Count);

                    switch (mood.Name)
                    {
                        case "Плохое":
                        {
                            dataset.BackgroundColor.Add("rgb(205, 92, 92)");
                            break;
                        }
                        case "Сонное":
                        {
                            dataset.BackgroundColor.Add("rgb(219, 112, 147)");
                            break;
                        }
                        case "Нормальное":
                        {
                            dataset.BackgroundColor.Add("rgb(189, 195, 199)");
                            break;
                        }
                        case "Веселое":
                        {
                            dataset.BackgroundColor.Add("rgb(39, 174, 96)");
                            break;
                        }
                        case "Отличное":
                        {
                            dataset.BackgroundColor.Add("rgb(70, 130, 180)");
                            break;
                        }
                    }
                }
            }
          
            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;
            return chart;
        }

        public Chart GetYearMood()
        {
            Year currentyear = new Year(DateTime.Now.Year);

            Chart chart = new Chart();
            chart.Type = "bar";

            Data data = new Data();
            data.Labels = new List<string>();

            List<Dataset> datasetList = new List<Dataset>();

            var userId = userManager.GetUserId(User);
            var moods = db.UserMoods
                .Where(m => m.UserId == userId && m.Date.Year == currentyear.YearId).Include(m => m.Mood)
                .GroupBy(m => m.Mood.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToList();

                foreach (var mood in moods)
                {
                    BarDataset dataset = new BarDataset()
                    {
                        Data = new List<double>(),
                        BackgroundColor = new List<string>()
                    };

                    dataset.Label = mood.Name;
                    dataset.Data.Add(mood.Count);

                    switch (mood.Name)
                    {
                        case "Плохое":
                        {
                            dataset.BackgroundColor.Add("rgb(205, 92, 92)");
                            break;
                        }
                        case "Сонное":
                        {
                            dataset.BackgroundColor.Add("rgb(219, 112, 147)");
                            break;
                        }
                        case "Нормальное":
                        {
                            dataset.BackgroundColor.Add("rgb(189, 195, 199)");
                            break;
                        }
                        case "Веселое":
                        {
                            dataset.BackgroundColor.Add("rgb(39, 174, 96)");
                            break;
                        }
                        case "Отличное":
                        {
                            dataset.BackgroundColor.Add("rgb(70, 130, 180)");
                            break;
                        }          
                    }

                    datasetList.Add(dataset);
                } 

            data.Datasets = datasetList;

            chart.Data = data;

            BarOptions options = new BarOptions()
            {
                Scales = new Scales(),
                BarPercentage = 1
            };

            Scales scales = new Scales()
            {
                YAxes = new List<Scale>()
                {
                    new CartesianScale()
                    {
                        Ticks = new CartesianLinearTick()
                        {
                            BeginAtZero = true
                        }
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout()
            {
                Padding = new Padding()
                {
                    PaddingObject = new PaddingObject()
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };
            return chart;
        }
      
    }
}
