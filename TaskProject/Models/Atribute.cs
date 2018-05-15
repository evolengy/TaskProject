using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    public class Atribute
    {
        public Atribute()
        {
            Lvl = 1;
            CurrentExp = 0;
            MaxExp = 500;

            Skills = new List<Skill>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AtributeId { get; set; }
        [Display(Name = "Навык")]
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int Lvl { get; set; }

        public int CurrentExp { get; set; }
        public int MaxExp { get; set; }

        public virtual List<Skill> Skills { get; set; }
    }
}
