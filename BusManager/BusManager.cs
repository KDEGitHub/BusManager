﻿using BusManager.Configuration;
using BusManager.Connection;
using BusManager.Queue;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusManager
{
    public class BusManager : IBusManager
    {
        private readonly IBusConnection _connection;
        private readonly ILogger<BusManager> _logger;
        private readonly IBusConfiguration _config;
        private readonly List<IBusQueue> _queueList = new List<IBusQueue>();

        public BusManager(IBusConnection connection, IBusConfiguration config, ILogger<BusManager> logger = null)
        {
            _config = config;
            _connection = connection;
            _logger = logger;
            InitQueues();
        }

        public IBusQueue GetQueue(string serviceName)
        {
            try
            {
                return _queueList.FirstOrDefault(item => item.ServiceName == serviceName);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, $"{_config.LocalIP} : {_config.ApplicationName}.{typeof(BusManager).FullName} : {e.Message}");
                return null;
            }
        }
        
        public void InitQueues()
        {
            try
            {
                if (_config.Services == null) return;

                if (_connection.TryConnect())
                    foreach (var serviceConfiguration in _config.Services)
                        _queueList.Add(new BusQueue(_connection, serviceConfiguration, _logger));
            }
            catch (Exception e)
            {
                _logger?.LogError(e, $"{_config.LocalIP} : {_config.ApplicationName}.{typeof(BusManager).FullName} : {e.Message}");
            }
        }
        
        public void DestroyQueues()
        {
            try
            {
                foreach (IBusQueue item in _queueList)
                {
                    item.Dispose();
                }
                _queueList.Clear();
                _connection.Dispose();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, $"{_config.LocalIP} : {_config.ApplicationName}.{typeof(BusManager).FullName} : {e.Message}");
            }
        }

        public bool IsInitQueue(string serviceName)
        {
            try
            {
                IBusQueue queue = GetQueue(serviceName);
                if (queue == null)
                {
                    IServiceConfiguration config = _config.Services.FirstOrDefault(x => x.ServiceName == serviceName);
                    if (config != null)
                    {
                        queue = new BusQueue(_connection, config, _logger);
                        _queueList.Add(queue);
                    }
                    if (queue == null) return false;
                }
                return queue.TryInit();
            }
            catch (Exception e)
            {
                _logger?.LogError(e, $"{_config.LocalIP} : {_config.ApplicationName}.{typeof(BusManager).FullName} : {e.Message}");
                return false;
            }

        }

        public bool IsInitQueues()
        {
            if (_config.Services == null) return true;
            
            if (_connection.TryConnect())
                foreach (var serviceConfiguration in _config.Services)
                {
                    IBusQueue queue = GetQueue(serviceConfiguration.ServiceName);
                    if (queue == null || queue.TryInit()) return false;
                }
            return false;
        }

     

    }
}
