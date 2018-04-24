using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class UserAtribute
    {
        public int UserAtributeId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Atribute")]
        public int AtributeId { get; set; }
        public virtual Atribute Atribute { get; set; }

        public int Value { get; set; } = 0;
        [Range(0, 10)]
        public int MaxValue { get; set; } = 10;

        public int CurrentExp { get; set; }
        public int MaxExp { get; set; }

    }
    public class Atribute
    {
        public int AtributeId { get; set; }
        [Display(Name = "Навык")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
