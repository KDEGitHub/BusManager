using BusManager.Configuration;
using BusManager.Connection;
using BusManager.Listener;
using BusManager.Logger;
using BusManager.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using BusManager.Model;

namespace BusManager.Receiver
{
    public class BusReceiver : IBusReceiver
    {
        private readonly IQueueConfiguration _config;
        private readonly IBusConnection _connection;
        private readonly IBusLogger _logger;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        public bool IsOpenChanel
        {
            get { return (_channel != null && _channel.IsOpen && _connection.TryConnect()); }
        }

        public BusReceiver(IBusConnection connection, IBusLogger logger, IQueueConfiguration config)
        {
            _connection = connection;
            _config = config;
            _logger = logger;
            TryCreateChannel();
        }

        public IBusListener GetListener()
        {
            if (TryCreateConsumer())
            {
                return new BusListener(_consumer, _logger);
            }
            return null;
        }

        public async Task<IBusMessage> ReceiveAsync(IBusMessage request, CancellationToken stoppingToken)
        {
            if (!TryCreateChannel())
                return null;
            using (BusListener listener = new BusListener(_consumer, _logger))
            {
                return await listener.GetResponseAsync(request.Id, stoppingToken);
            }
        }

        public bool TryCreateChannel()
        {
            try
            {
                if (_connection.TryConnect() && !IsOpenChanel)
                {
                    _channel = InitChannel();
                    TryCreateConsumer();
                }
            }
            catch (Exception e)
            {
                _logger.Push(new LoggerMessage()
                {
                    Message = e.Message,
                    Type = "Error",
                    Trace = typeof(BusReceiver).FullName
                });
                _channel = null;
            }
            return IsOpenChanel;
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }

        private bool TryCreateConsumer()
        {
            try
            {
                if (TryCreateChannel())
                {
                    if (_consumer == null)
                    {
                        _consumer = new EventingBasicConsumer(_channel);
                        _channel.BasicConsume(_config.Name,
                            autoAck: true,
                            consumer: _consumer);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Push(new LoggerMessage()
                {
                    Message = e.Message,
                    Type = "Error",
                    Trace = typeof(BusReceiver).FullName
                });
                _consumer = null;
                return false;
            }
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
