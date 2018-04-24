using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Guild
    {
        public Guild()
        {
            Missions = new List<Mission>();
        }
        public int GuildId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Mission> Missions { get; set; }
    }

    public class GuildsReputation
    {
        [Key]
        public int GuildReputationId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Guild")]
        public int GuildId { get; set; }
        public Guild Guild { get; set; }


        public int CurrentValue { get; set; } = 0;
        public int MaxValue { get; set; } = 2000;
    }
}
