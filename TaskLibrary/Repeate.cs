using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Repeat
    {
        public int RepeatId { get; set; }
        [Display(Name = "Повтор")]
        public string Name { get; set; }
    }
}
