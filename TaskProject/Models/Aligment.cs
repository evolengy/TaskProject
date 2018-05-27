using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskProject.Models
{
    public class Aligment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AligmentId { get; set; }

        [Display(Name="Мировозрение")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}
