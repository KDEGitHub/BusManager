using System;
using BusManager.Messages;
using System.Threading.Tasks;
using BusManager.Listener;

namespace BusManager.Queue
{
    /// <summary>
    /// интерфейс очереди по зваимодействию между сервисами
    /// </summary>
    public interface IBusQueue : IDisposable
    {
        /// <summary>
        /// наименование сервиса с которым взаимодействует очередь
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// попытка инициализации очереди для обменя данными 
        /// </summary>
        /// <returns>результат инициализации</returns>
        bool TryInit();

        /// <summary>
        /// плучение слушателя очереди 
        /// </summary>
        /// <returns></returns>
        IBusListener GetListener();

        /// <summary>
        /// отправка сообщения на шину без получения ответа
        /// </summary>
        /// <param name="request">сообщение</param>
        bool Push(IBusMessage request);

        /// <summary>
        ///  отсправка сообщения и ожидание ответа 
        /// </summary>
        /// <param name="request">сообщение</param>
        /// <returns>сообщение с ответом</returns>
        Task<IBusMessage> ReciveAsync(IBusMessage request);

    }
}
