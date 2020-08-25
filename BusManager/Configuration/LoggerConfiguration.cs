namespace BusManager.Configuration
{
    public class LoggerConfiguration : ILoggerConfiguration
    {
        public string ApplicationName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public int NetworkRecoveryInterval { get; set; }
        public QueueConfiguration Producer { get; set; }
    }
}
