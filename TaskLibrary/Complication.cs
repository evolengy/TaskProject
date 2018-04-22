using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Complication
    {
        public int ComplicationId { get; set; }
        [Display(Name = "Сложность")]
        public string Name { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }
        public int Damage { get; set; }
    }
}
