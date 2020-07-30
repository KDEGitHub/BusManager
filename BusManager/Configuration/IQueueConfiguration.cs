namespace BusManager.Configuration
{
    public interface IQueueConfiguration
    {
        string Name { get; set; }
        bool Durable { get; set; }
        bool Exclusive { get; set; }
        bool AutoDelete { get; set; }
        ExchangeConfiguration Exchange { get; set; }
    }
}
