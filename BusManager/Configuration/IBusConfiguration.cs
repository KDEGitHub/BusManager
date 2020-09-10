using System.Collections.Generic;

namespace BusManager.Configuration
{
    public interface IBusConfiguration
    {
        string ApplicationName { get; set; }
        string LocalIP { get; }
        string UserName { get; set; }
        string Password { get; set; }
        string HostName { get; set; }
        int Port { get; set; }
        string VirtualHost { get; set; }
        bool AutomaticRecoveryEnabled { get; set; }
        int NetworkRecoveryInterval { get; set; }
        List<ServiceConfiguration> Services { get; set; }
    }
}
