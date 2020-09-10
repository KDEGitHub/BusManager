using BusManager.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace BusManager.Connection
{
    public class BusConnection : IBusConnection
    {
        private readonly IBusConfiguration _settings;
        private readonly ILogger<BusConnection> _logger;
        public IConnection Connection { get; private set; }
        public bool IsConnected
        {
            get { return (Connection != null && Connection.IsOpen); }
        }

        public string ServerInfo
        {
            get { return $"{_settings.LocalIP} : {_settings.ApplicationName}"; }
        }

        public BusConnection(IBusConfiguration settings, ILogger logger = null)
        {
            _settings = settings;
            _logger = (ILogger<BusConnection>)logger;
            TryConnect();
        }
        
        public void Dispose()
        {
            Connection?.Dispose();
        }
        
        public bool TryConnect()
        {
            try
            {
                if (!IsConnected)
                {
                    ConnectionFactory factory = new ConnectionFactory()
                    {
                        HostName = _settings.HostName,
                        Port = _settings.Port,
                        UserName = _settings.UserName,
                        Password = _settings.Password,
                        VirtualHost = _settings.VirtualHost,
                        AutomaticRecoveryEnabled = _settings.AutomaticRecoveryEnabled,
                        NetworkRecoveryInterval = TimeSpan.FromSeconds(_settings.NetworkRecoveryInterval)
                    };
                    Connection = factory.CreateConnection();
                }
            }
            catch (Exception e)
            {
                _logger?.LogError(e, $"{ServerInfo}.{typeof(BusConnection).FullName} : {e.Message}");
                Connection = null;
            }
            return IsConnected;
        }

    }
}
