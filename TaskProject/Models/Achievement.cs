using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    public class UserAchievement
    {
        public UserAchievement()
        {
            IsOpen = false;
            SetDate = null;
        }

        public int UserAchievementId { get; set; }

        public DateTime? SetDate { get; set; }
        public bool IsOpen { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Achievement")]
        public int AchievementId { get; set; }
        public Achievement Achievement { get; set; }
    }

    public class Achievement
    {
        public int AchievementId { get; set; } 

        [Display(Name="Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public string LinkImg { get; set; }
    }
}
