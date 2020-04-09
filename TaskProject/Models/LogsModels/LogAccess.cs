using System;

namespace TaskProject.Models.LogsModels
{
	public class LogAccess
    {
        public int LogAccessId { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Log { get; set; }
        public string UserAgent { get; set; }
    }
}
