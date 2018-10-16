using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models.AchievementModels
{
    public class Achievement
    {
        public int AchievementId { get; set; } 

        [Display(Name="Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public string LinkImg { get; set; }

        public int AchievementType { get; set; }
    }

    public enum AchievementType
    {
        UpLevel2,
        UpLevel5,
        UpLevel10,
        UpLevel15,
        UpLevel20,
        UpGold500,
        UpGold1000,
        UpGold5000,
        UpGold10000,
        UpGoals10,
        UpGoals100,
        UpGoals1000,
        UpGoals5000,
        AddGoals10,
        AddGoals100,
        AddGoals1000,
        AddGoals5000,
        UpLevelSkill5,
        UpLevelSkill10,
        UpLevelSkill25,
        UpLevelSkill50,
        UpLevelAtribute5,
        UpLevelAtribute10,
        UpLevelAtribute25,
        UpLevelAtribute50,
        UpLevelAtribute100
    }

   
                 
}
