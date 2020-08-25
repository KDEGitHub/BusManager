using BusManager.Configuration;
using BusManager.Messages;
using BusManager.Model;
using RabbitMQ.Client;
using System;
using System.Net;
using System.Net.Sockets;

namespace BusManager.Logger
{
    public class BusLogger : IBusLogger
    {
        private readonly ILoggerConfiguration _settings;
        private IConnection _connection;
        private IModel _channel;
        private readonly string _ipAddress;
        private bool IsConnected
        {
            get { return (_connection != null && _connection.IsOpen); }
        }
        private bool IsOpenChannel
        {
            get { return (_channel != null && _channel.IsOpen); }
        }

        public BusLogger(ILoggerConfiguration settings)
        {
            _settings = settings;
            _ipAddress = GetLocalIPAddress();
            TryConnect();
        }

        public void Push(LoggerMessage message)
        {
            if (TryConnect() && TryCreateChannel())
            {
                message.Trace = $"[{_ipAddress}] {_settings.ApplicationName}.{message.Trace}";

                IBusMessage request = new BusMessage
                {
                    Id = Guid.NewGuid(),
                    MessageType = "LoggerService.WriteMessage",
                    Body = message
                };
                _channel.BasicPublish(exchange: _settings.Producer.Exchange.Name,
                    routingKey: _settings.Producer.Exchange.RoutingKey,
                    basicProperties: null,
                    body: request.GetMessage());

            }
        }
        
        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
        
        private bool TryConnect()
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
                    _connection = factory.CreateConnection();
                }
            }
            catch
            {
                _connection = null;
            }
            return IsConnected;
        }
        
        private bool TryCreateChannel()
        {
            try
            {
                if (!IsOpenChannel)
                {
                    if (TryConnect())
                        _channel = InitChannel();
                }
            }
            catch
            {
                _channel = null;
            }

            return IsOpenChannel;
        }
        
        private IModel InitChannel()
        {
            IModel channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: _settings.Producer.Exchange.Name,
                type: _settings.Producer.Exchange.Type,
                _settings.Producer.Exchange.Durable,
                _settings.Producer.Exchange.AutoDelete,
                null);
            channel.QueueDeclare(_settings.Producer.Name,
                durable: _settings.Producer.Durable,
                exclusive: _settings.Producer.Exclusive,
                autoDelete: _settings.Producer.AutoDelete,
                arguments: null);
            channel.QueueBind(queue: _settings.Producer.Name,
                exchange: _settings.Producer.Exchange.Name,
                routingKey: _settings.Producer.Exchange.RoutingKey);
            return channel;
        }

        private string GetLocalIPAddress()
        {

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }

    }

}
