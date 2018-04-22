using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLibrary
{
    public class Attainment
    {
        public int AttainmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LinkImage { get; set; }

        public List<Character> Character { get; set; }
    }
}
