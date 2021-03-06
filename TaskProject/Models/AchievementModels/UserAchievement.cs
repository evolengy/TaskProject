﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProject.Models.AchievementModels
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
}
