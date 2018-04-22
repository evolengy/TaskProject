using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TaskLibrary;

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
            if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
            {
                return RedirectToAction("LogOff","Account");
            }
            int characterid = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());
            Character Character = db.Characters.Where(c => c.CharacterId == characterid).SingleOrDefault();

            foreach (Guild Guild in Guilds)
            {
                if(!Character.GuildsRep.Exists(g => g.GuildId == Guild.GuildId))
                {
                    GuildsReputation GuildRep = new GuildsReputation()
                    {
                        GuildId = Guild.GuildId
                    };
                    Character.GuildsRep.Add(GuildRep);
                    db.SaveChanges();
                }
            }
            return View(Character.GuildsRep);
        }

        public ActionResult GetMissions(int id)
        {
            List<Mission> Missions = db.Missions.Where(m => m.GuildId == id).ToList();

            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
            {
                return RedirectToAction("LogOff", "Account");
            }
            int characterid = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());
            Character Character = db.Characters.Where(c => c.CharacterId == characterid).SingleOrDefault();

            foreach(Mission Mission in Missions)
            {
                if (!Character.Missions.Exists(m => m.MissionId == Mission.MissionId))
                {
                    Character.Missions.Add(new MissionsCondition()
                    {
                        MissionId = Mission.MissionId
                    });
                    db.SaveChanges();
                }
            }
            return View(Character.Missions);
        }

        public ActionResult StartMission(int id)
        {
            int result;
            if (!Int32.TryParse(HttpContext.Request.Cookies["CharacterId"].ToString(), out result))
            {
                return RedirectToAction("LogOff", "Account");
            }
            int characterid = Int32.Parse(HttpContext.Request.Cookies["CharacterId"].ToString());
            Character Character = db.Characters.Where(c => c.CharacterId == characterid).SingleOrDefault();
            MissionsCondition MissionCondition = Character.Missions.Where(m => m.MissionConditionId == id).SingleOrDefault();
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

            mission.Character.CurrentExp = mission.Character.CurrentExp + mission.Mission.LevelUp;
            if (mission.Character.CurrentExp >= mission.Character.MaxExp)
            {
                mission.Character.CurrentLevel++;
                mission.Character.CurrentExp = 0;
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