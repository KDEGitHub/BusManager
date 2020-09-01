using BusManager.Configuration;
using BusManager.Connection;
using BusManager.Helpers;
using BusManager.Listener;
using BusManager.Logger;
using BusManager.Messages;
using BusManager.Producer;
using BusManager.Receiver;
using System;
using System.Threading;
using System.Threading.Tasks;
using BusManager.Model;

namespace BusManager.Queue
{
    public class BusQueue : IBusQueue
    {
        private readonly IBusConnection _connection;
        private readonly IServiceConfiguration _config;
        private readonly IBusLogger _logger;
        private IBusReceiver _receiver;
        private IBusProducer _producer;
        public string ServiceName
        {
            get { return _config.ServiceName; }
        }

        public BusQueue(IBusConnection connection, IServiceConfiguration config, IBusLogger logger= null)
        {
            _connection = connection;
            _config = config;
            _logger = logger;
            TryInit();
        }

        public IBusListener GetListener()
        {
            return _receiver?.GetListener();
        }

        public async Task<IBusMessage> ReciveAsync(IBusMessage request)
        {
            try
            {
                if (!_producer.Push(request))
                    throw new Exception("Error: message not sent!");

                CancellationToken token = TokenHelper.GetToken(_config.ResponseTimeout);
                var result = await _receiver.ReceiveAsync(request, token);
                if (result == null)
                {
                    result = request;
                    result.Body = null;
                }
                return result;
            }
            catch (Exception e)
            {
                _logger?.Push(new LoggerMessage()
                {
                    Message = e.Message,
                    Type = "Error",
                    Trace = typeof(BusQueue).FullName
                });
                request.IsError = true;
                request.ErrorDetails = e.Message;
                return request;
            }
        }
        
        public bool Push(IBusMessage request)
        {
            return _producer.Push(request);
        }

        public bool TryInit()
        {
            try
            {
                if (_config.Receiver != null)
                {
                    if (_receiver == null)
                        _receiver = new BusReceiver(_connection,_config.Receiver, _logger);

                    if (!_receiver.TryCreateChannel())
                        return false;
                }

                if (_config.Producer != null)
                {
                    if (_producer == null)
                        _producer = new BusProducer(_connection, _config.Producer, _logger);

                    if (!_producer.TryCreateChannel())
                        return false;
                }

                return true;


            }
            catch (Exception e)
            {
                _logger?.Push(new LoggerMessage()
                {
                    Message = e.Message,
                    Type = "Error",
                    Trace = typeof(BusQueue).FullName
                });
                return false;
            }
        }

        public void Dispose()
        {
            _receiver?.Dispose();
            _producer?.Dispose();
        }
        
    }
}
