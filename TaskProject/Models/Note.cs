using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    public class Note
    {
        public Note()
        {
            DateCreate = DateTime.Now.Date;
        }

        public int NoteId { get; set; }
        public string Text { get; set; }
        public DateTime DateCreate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
