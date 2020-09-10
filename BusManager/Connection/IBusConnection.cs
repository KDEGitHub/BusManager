using System;
using RabbitMQ.Client;

namespace BusManager.Connection
{
    /// <summary>
    /// интерфейс соединения с шиной данных 
    /// </summary>
    public interface IBusConnection : IDisposable
    {
        /// <summary>
        /// соединение 
        /// </summary>
        IConnection Connection { get; }
        /// <summary>
        /// состояние соединения 
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// Наименование сервиса плюс ip адрес 
        /// </summary>
        string ServerInfo { get; }
        /// <summary>
        /// попытка соединения 
        /// </summary>
        /// <returns>результат соединения</returns>
        bool TryConnect();
    }
}
