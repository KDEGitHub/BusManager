using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace BusManager.Configuration
{
    public abstract class BusConfiguration : IBusConfiguration
    {
        public string ApplicationName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public int NetworkRecoveryInterval { get; set; }
        public List<ServiceConfiguration> Services { get; set; }
        
        public string LocalIP {
            get { return GetLocalIPAddress(); }
        }
        
        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                return "";
            }
        }

    }
}
