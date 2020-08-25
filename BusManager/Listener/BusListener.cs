using BusManager.Logger;
using BusManager.Messages;
using BusManager.Model;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusManager.Listener
{
    public class BusListener : IBusListener
    {
        private readonly IBusLogger _logger;
        public readonly EventingBasicConsumer _consumer;
        private readonly BlockingCollection<IBusMessage> _queueMessages = new BlockingCollection<IBusMessage>();
        private Guid _messageId;
        
        public BusListener(EventingBasicConsumer consumer, IBusLogger logger = null)
        {
            _consumer = consumer;
            _logger = logger;
        }
        public void Subscribe(MessageReceived sb)
        {
            _consumer.Received += (model, eventArgument) =>
            {
                sb?.Invoke(model, eventArgument);
            };
        }

        public async Task<IBusMessage> GetResponseAsync(Guid messageId, CancellationToken stoppingToken)
        {
            _messageId = messageId;
            _consumer.Received += MessageReceived;
            return _queueMessages.Take(stoppingToken);
        }

        public void Dispose()
        {
            _consumer.Received -= MessageReceived;
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs eventArgument)
        {
            try
            {
                byte[] body = eventArgument.Body.ToArray();
                string bytesAsString = Encoding.UTF8.GetString(body);
                IBusMessage item = JsonConvert.DeserializeObject<BusMessage>(bytesAsString);
                if (item.Id.Equals(_messageId))
                    _queueMessages.Add(item);
            }
            catch (JsonException e)
            {
                _logger?.Push(new LoggerMessage()
                {
                    Message = e.Message,
                    Type = "Error",
                    Trace = typeof(BusListener).FullName
                });
            }
        }

       
    }
}
