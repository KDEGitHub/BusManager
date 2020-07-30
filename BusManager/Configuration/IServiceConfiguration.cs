namespace BusManager.Configuration
{
    public interface IServiceConfiguration
    {
        string ServiceName { get; set; }
        int ResponseTimeout { get; set; }
        QueueConfiguration Producer { get; set; }
        QueueConfiguration Receiver { get; set; }
    }
}
