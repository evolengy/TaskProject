using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProject.Models
{
	public class UserReward
    {
        public int UserRewardId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Название не задано")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Стоимость")]
        public int? Cost { get; set; } 

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
