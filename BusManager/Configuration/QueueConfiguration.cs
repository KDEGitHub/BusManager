namespace BusManager.Configuration
{
    public class QueueConfiguration : IQueueConfiguration
    {
        public string Name { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public ExchangeConfiguration Exchange { get; set; }
    }
}
