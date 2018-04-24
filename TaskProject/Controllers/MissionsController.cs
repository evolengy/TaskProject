using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TaskLibrary;
using TaskProject.Models;

namespace Spacewander.Controllers
{
    public class MissionsController : Controller
    {
        private ApplicationDbContext db;

        public ActionResult Guilds(ApplicationDbContext _db)
        {
            db = _db;
            List<Guild> Guilds = db.Guilds.ToList();
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
            {
                return RedirectToAction("LogOff","Account");
            }
            string id = HttpContext.Request.Cookies["UserId"];
            ApplicationUser User = db.Users.Where(c => c.Id == id).SingleOrDefault();

            foreach (Guild Guild in Guilds)
            {
                if(!User.GuildsRep.Exists(g => g.GuildId == Guild.GuildId))
                {
                    GuildsReputation GuildRep = new GuildsReputation()
                    {
                        GuildId = Guild.GuildId
                    };
                    User.GuildsRep.Add(GuildRep);
                    db.SaveChanges();
                }
            }
            return View(User.GuildsRep);
        }

        public ActionResult GetMissions(int missionid)
        {
            List<Mission> Missions = db.Missions.Where(m => m.GuildId == missionid).ToList();

            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
            {
                return RedirectToAction("LogOff", "Account");
            }
            string userid = HttpContext.Request.Cookies["UserId"];
            ApplicationUser User = db.Users.Where(c => c.Id == userid).SingleOrDefault();

            foreach(Mission Mission in Missions)
            {
                if (!User.Missions.Exists(m => m.MissionId == Mission.MissionId))
                {
                    User.Missions.Add(new MissionsCondition()
                    {
                        MissionId = Mission.MissionId
                    });
                    db.SaveChanges();
                }
            }
            return View(User.Missions);
        }

        public ActionResult StartMission(int id)
        {
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["UserId"].ToString(), out result))
            {
                return RedirectToAction("LogOff", "Account");
            }
            string userid = HttpContext.Request.Cookies["UserId"];

            ApplicationUser User = db.Users.Where(c => c.Id == userid).SingleOrDefault();
            MissionsCondition MissionCondition = User.Missions.Where(m => m.MissionConditionId == id).SingleOrDefault();
            MissionCondition.IsAccepted = true;
            db.SaveChanges();
            return RedirectToAction("GameRoom", "Home");
        }

        public ActionResult CompleteMission(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            MissionsCondition mission = db.MissionsConditions.Find(id);
            if (mission == null)
            {
                return NotFound();
            }

            mission.IsComplete = true;
            mission.IsAccepted = false;

            mission.User.CurrentExp = mission.User.CurrentExp + mission.Mission.LevelUp;
            if (mission.User.CurrentExp >= mission.User.MaxExp)
            {
                mission.User.CurrentLevel++;
                mission.User.CurrentExp = 0;
            }

            GuildsReputation guild = db.GuildsReputations.Where(g => g.GuildId == mission.Mission.GuildId).SingleOrDefault();
            guild.CurrentValue = guild.CurrentValue + mission.Mission.RepUp;

            if(guild.CurrentValue >= guild.MaxValue)
            {
                guild.CurrentValue = guild.MaxValue;
            }
            db.SaveChanges();
            return RedirectToAction("GameRoom", "Home");
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