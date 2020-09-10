using BusManager.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BusManager.Listener
{
    public class BusListener : IBusListener
    {
       
        private readonly ILogger<BusListener> _logger;
        public readonly EventingBasicConsumer _consumer;
        private readonly BlockingCollection<IBusMessage> _queueMessages = new BlockingCollection<IBusMessage>();
        private Guid _messageId;
        private readonly string _serverInfo;


        public BusListener(EventingBasicConsumer consumer, ILogger logger = null, string serverInfo = "")
        {
            _consumer = consumer;
            _logger = (ILogger<BusListener>)logger;
            _serverInfo = serverInfo;
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
                _logger?.LogError(e, $"{_serverInfo}.{typeof(BusListener).FullName} : {e.Message}");
            }
        }

       
    }
}
