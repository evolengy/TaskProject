using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProject.Models.GoalModels
{
	public class Catalog
    {
        public Catalog()
        {
            Goals = new List<Goal>();
        }

        public int CatalogId { get; set; }

        [Required]
        [Display(Name = "Имя"), MaxLength(30, ErrorMessage = "Превышено максимальное количество знаков в названии списка - 30")]
        public string Name { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public List<Goal> Goals { get; set; }
    }
}
