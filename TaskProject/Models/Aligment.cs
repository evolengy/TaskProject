using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskProject.Models
{
    public class Aligment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AligmentId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
