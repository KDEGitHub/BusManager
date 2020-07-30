using System;
using BusManager.Model;

namespace BusManager.Logger
{
    /// <summary>
    /// интерфейс сервиса логирования 
    /// </summary>
    public interface IBusLogger : IDisposable
    {
        /// <summary>
        /// запись сообщения
        /// </summary>
        /// <param name="message">сообщение в лог</param>
        void Push(LoggerMessage message);
    }
}
