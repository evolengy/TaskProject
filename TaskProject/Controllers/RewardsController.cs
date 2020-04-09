using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskProject.Models;

namespace TaskProject.Controllers
{
	public class RewardsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public RewardsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public async Task<IActionResult> GetRewards()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            var rewards = await db.UserRewards.Where(r => r.UserId == user.Id).ToListAsync();

            ViewBag.Gold = user.CurrentGold;
            ViewBag.BreadCrumb = "Награды";
            return View(rewards);
        }

        public IActionResult AddReward()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReward(UserReward reward)
        {
            if (reward.Cost == null)
            {
                reward.Cost = 0;
            }
            else if(reward.Cost < 0)
            {
                ModelState.AddModelError("Cost", "Значение не может быть меньше 0");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if(user == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                reward.UserId = user.Id;

                await db.AddAsync(reward);

                await db.Notifications.AddAsync(new Notification()
                {
                    Name = "Добавлена новая награда: " + reward.Name,
                    DateCreate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now),
                    UserId = user.Id
                });

                await db.SaveChangesAsync();

                ViewBag.Message = "Награда успешно добавлена.";
                return PartialView("_Success");
            }

            return PartialView(reward);
        }


        public async Task<IActionResult> EditReward(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reward = await db.UserRewards.SingleOrDefaultAsync(m => m.UserRewardId == id);
            if (reward == null)
            {
                return NotFound();
            }

            return PartialView(reward);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReward(UserReward reward)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(reward);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RewardExists(reward.UserRewardId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.Message = "Награда успешно изменена.";
                return PartialView("_Success");
            }
            return PartialView(reward);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReward(int? id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Logout", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var reward = await db.UserRewards.SingleOrDefaultAsync(m => m.UserRewardId == id);

            if (reward == null)
            {
                return NotFound();
            }

            db.Notifications.Add(new Notification()
            {
                Name = "Удалена награда: " + reward.Name,
                DateCreate = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now),
                UserId = reward.UserId
            });

            db.UserRewards.Remove(reward);
            await db.SaveChangesAsync();

            return Ok();
        }


        public async Task<IActionResult> CompleteReward(int? id)
        {
            var user = await db.Users.Where(u => u.Id == userManager.GetUserId(User)).SingleOrDefaultAsync();
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var reward = await db.UserRewards.Where(r => r.UserRewardId == id).SingleOrDefaultAsync();
            if (reward == null)
            {
                return NotFound();
            }

            if(reward.Cost < user.CurrentGold)
            {
                user.CurrentGold -= (int)reward.Cost;
            }

            await db.SaveChangesAsync();

            return RedirectToAction("GetRewards");
        }

        private bool RewardExists(int id)
        {
            return db.UserRewards.Any(e => e.UserRewardId == id);
        }
    }
}
