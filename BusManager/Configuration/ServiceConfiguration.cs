namespace BusManager.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public string ServiceName { get; set; }
        public int ResponseTimeout { get; set; }
        public QueueConfiguration Producer { get; set; }
        public QueueConfiguration Receiver { get; set; }
    }
}
