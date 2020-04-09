using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProject.Models.GoalModels
{
	public class Repeat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RepeatId { get; set; }
        [Display(Name = "Повтор")]
        public string Name { get; set; }
    }
}
