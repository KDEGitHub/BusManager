namespace BusManager.Configuration
{
    public class ExchangeConfiguration
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public string RoutingKey { get; set; }
    }
}
