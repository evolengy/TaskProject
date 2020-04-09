using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProject.Models
{
	public class Notification
    {
        public int NotificationID { get; set; }

        public string Name { get; set; }

        public DateTime DateCreate { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }


    public class NotificationEventArgs
    {
        public string Name { get; }
        public string UserId { get; }

        public NotificationEventArgs(string name, string userid)
        {
            Name = name;
            UserId = userid;
        }
    }
}
