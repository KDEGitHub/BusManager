using System;
using BusManager.Messages;

namespace BusManager.Producer
{
    /// <summary>
    /// интерфейс класса для отправки сообщений на шину данных 
    /// </summary>
    public interface IBusProducer : IDisposable
    {
        /// <summary>
        /// попытка создания канала для обмена с шиной 
        /// </summary>
        /// <returns>результат</returns>
        bool TryCreateChannel();
        
        /// <summary>
        /// Отправка сообщения в очередь 
        /// </summary>
        /// <param name="request">сообщение</param>
        /// <returns>результат отправки true/false</returns>
        bool Push(IBusMessage request);
    }
}
