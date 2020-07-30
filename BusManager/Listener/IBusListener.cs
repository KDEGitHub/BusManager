using BusManager.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace BusManager.Listener
{
    /// <summary>
    /// делегат для подписки на получение сообщений 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgument"></param>
    public delegate void MessageReceived(object sender, BasicDeliverEventArgs eventArgument);
    /// <summary>
    /// интерфейс класса для получения ответа на посланное сообщение в очередь 
    /// </summary>
    public interface IBusListener : IDisposable
    {
        void Subscribe(MessageReceived func);

        /// <summary>
        ///  получение ответа на сообщение из очереди 
        /// </summary>
        /// <param name="messageId"> идентификатор сообщения</param>
        /// <param name="stoppingToken">токен отмены</param>
        /// <returns> ответ на сообщение </returns>
        Task<IBusMessage> GetResponseAsync(Guid messageId, CancellationToken stoppingToken);

    }
}
