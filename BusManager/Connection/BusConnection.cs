using BusManager.Configuration;
using RabbitMQ.Client;
using System;
using BusManager.Logger;
using BusManager.Model;

namespace BusManager.Connection
{
    public class BusConnection : IBusConnection
    {
        private readonly IBusConfiguration _settings;
        private readonly IBusLogger _logger;
        public IConnection Connection { get; private set; }
        public bool IsConnected
        {
            get { return (Connection != null && Connection.IsOpen); }
        }

        public BusConnection(IBusConfiguration settings, IBusLogger logger)
        {
            _settings = settings;
            _logger = logger;
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
                _logger?.Push(new LoggerMessage()
                {
                    Message = e.Message,
                    Type = "Error",
                    Trace = typeof(BusConnection).FullName
                });
                Connection = null;
            }
            return IsConnected;
        }

    }
}
