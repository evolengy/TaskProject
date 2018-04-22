using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Mission
    {
        public int MissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelUnlock { get; set; }

        [ForeignKey("Guild")]
        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public int RepUp { get; set; }
        public int LevelUp { get; set; }
    }

    public class MissionsCondition
    {
        [Key]
        public int MissionConditionId { get; set; }

        [ForeignKey("Character")]
        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }

        [ForeignKey("Mission")]
        public int MissionId { get; set; }
        public virtual Mission Mission { get; set; }

        public bool IsComplete { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
    }
}
