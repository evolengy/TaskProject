using System;

namespace TaskProject.Models.LogsModels
{
	public class LogEvent
    {
        public int LogEventId { get; set; }       
        public int EventId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }  
    }
}
