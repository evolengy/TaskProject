using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{

    public class UserMood
    {
        public UserMood()
        {
            Date = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Date;
        }

        public int UserMoodId { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        [ForeignKey("Mood")]
        public int MoodId { get; set; }
        public Mood Mood { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class Mood
    {
        public Mood()
        {
            UserMoods = new List<UserMood>();
        }

        public int MoodId { get; set; }
        [Required]
        public string Name { get; set; }
        public string LinkImg { get; set; }

        public virtual List<UserMood> UserMoods { get; set; }
    }
}
