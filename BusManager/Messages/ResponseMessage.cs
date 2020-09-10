using System;
using System.Collections.Generic;
using System.Text;

namespace BusManager.Messages
{
    /// <summary>
    /// Класс реализует ответное сообщение для шины данных
    /// </summary>
    public class ResponseMessage : BusMessage
    {
        /// <summary>
        /// Создает экземпляр из идентификатора и типа сообщения
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <param name="messageType">Тип сообщения</param>
        public ResponseMessage(Guid id, string messageType, int ttl = 30)
        {
            Id = id;
            MessageType = messageType;
            CreateDate = DateTime.UtcNow;
            TTL = ttl;
        }
    }
}
