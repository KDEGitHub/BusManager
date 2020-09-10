using BusManager.Configuration;
using BusManager.Connection;
using BusManager.Messages;
using RabbitMQ.Client;
using System;
using BusManager.Receiver;
using Microsoft.Extensions.Logging;

namespace BusManager.Producer
{
    public class BusProducer : IBusProducer
    {
        private readonly IQueueConfiguration _config;
        private readonly IBusConnection _connection;
        private readonly ILogger _logger;
        private IModel _channel;
        
        public bool IsOpenChanel
        {
            get { return (_channel != null && _channel.IsOpen && _connection.TryConnect()); }
        }

        public BusProducer(IBusConnection connection, IQueueConfiguration config, ILogger logger = null)
        {
            _connection = connection;
            _logger = logger;
            _config = config;
            TryCreateChannel();
        }

        public bool Push(IBusMessage request)
        {
            try
            {
                if (TryCreateChannel())
                {
                    _channel.BasicPublish(exchange: _config.Exchange.Name,
                        routingKey: _config.Exchange.RoutingKey,
                        basicProperties: null,
                        body: request.GetMessage());
                 
                    return true;

                }
                return false;
            }
            catch (Exception e)
            {
                _logger?.LogError(e, $"{_connection.ServerInfo}.{typeof(BusProducer).FullName} : {e.Message}");
                return false;
            }
        }
       
        public void Dispose()
        {
            _channel?.Dispose();
        }

        public bool TryCreateChannel()
        {
            try
            {
                if (_connection.TryConnect() && !IsOpenChanel)
                    _channel = InitChannel();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, $"{_connection.ServerInfo}.{typeof(BusProducer).FullName} : {e.Message}");
                _channel = null;
            }
            return IsOpenChanel;
        }

        private IModel InitChannel()
        {
            IModel channel = _connection.Connection.CreateModel();
            channel.ExchangeDeclare(exchange: _config.Exchange.Name,
                type: _config.Exchange.Type,
                _config.Exchange.Durable,
                _config.Exchange.AutoDelete,
                null);
            channel.QueueDeclare(_config.Name,
                durable: _config.Durable,
                exclusive: _config.Exclusive,
                autoDelete: _config.AutoDelete,
                arguments: null);
            channel.QueueBind(queue: _config.Name,
                exchange: _config.Exchange.Name,
                routingKey: _config.Exchange.RoutingKey);
            return channel;
        }
      
    }
}
