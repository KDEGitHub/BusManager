namespace BusManager.Configuration
{
    public interface ILoggerConfiguration
    {
        string UserName { get; set; }
        string Password { get; set; }
        string HostName { get; set; }
        int Port { get; set; }
        string VirtualHost { get; set; }
        bool AutomaticRecoveryEnabled { get; set; }
        int NetworkRecoveryInterval { get; set; }
        QueueConfiguration Producer { get; set; }
    }
}
