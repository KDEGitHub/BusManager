using System;

namespace BusManager.Model
{
    public class LoggerMessage
    {
        public string Trace { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }

        public LoggerMessage()
        {
            Created = DateTime.UtcNow;
        }
    }
}
