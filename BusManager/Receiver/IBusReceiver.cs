using BusManager.Listener;
using BusManager.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusManager.Receiver
{
    /// <summary>
    /// интерфейс по получению сообщений из очереди 
    /// </summary>
    public interface IBusReceiver : IDisposable
    {
        /// <summary>
        /// попытка создания канала для обмена с шиной 
        /// </summary>
        /// <returns>результат</returns>
        bool TryCreateChannel();

        /// <summary>
        /// плучение слушателя очереди 
        /// </summary>
        /// <returns></returns>
        IBusListener GetListener();

        /// <summary>
        /// Получить сообщение из очереди 
        /// </summary>
        /// <param name="request">сообщение для постановки в очередь</param>
        /// <param name="stoppingToken">токен отмены операции</param>
        /// <returns>ответ из шины данных</returns>
        Task<IBusMessage> ReceiveAsync(IBusMessage request, CancellationToken stoppingToken);
    }
}
