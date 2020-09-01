using System;

namespace BusManager.Messages
{
    /// <summary>
    /// интерфейс базового сообщения по взаимодействию с шиной 
    /// </summary>
    public interface IBusMessage
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        string MessageType { get; set; }

        /// <summary>
        /// Тело сообщения 
        /// </summary>
        object Body { get; set; }

        /// <summary>
        /// Наличие ошибок 
        /// </summary>
        bool IsError { get; set; }

        /// <summary>
        /// Детализация ошибки 
        /// </summary>
        string ErrorDetails { get; set; }

        /// <summary>
        /// Дата создания 
        /// </summary>
        DateTime CreateDate { get; set; }

        /// <summary>
        /// время жизни сообщения в сек.
        /// </summary>
        int TTL { get; set; }

        /// <summary>
        ///  идентификатор пользователя 
        /// </summary>
        Guid UserId { get; set; }

        /// <summary>
        /// олучить сконвертированное сообщение
        /// </summary>
        /// <returns> сообщение </returns>
        byte[] GetMessage();

        /// <summary>
        /// Приведение тела сообщения к определенному виду  
        /// </summary>
        /// <typeparam name="T">тип</typeparam>
        /// <returns></returns>
        T GetBody<T>();
    }
}
