using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BusManager.Messages;
using Microsoft.Extensions.Logging;

namespace BusManager.Helpers
{
    /// <summary>
    /// класс для извлечения ответа для обработки 
    /// </summary>
    public static class ResponseHelper
    {
        public static IBusMessage TryExecuteOperationWrapper<T>(IBusMessage item, Func<T, object> operation)
        {
            IBusMessage message = new ResponseMessage(item.Id, item.MessageType);
            try
            {
                T value = item.GetBody<T>();
                message.Body = operation(value);
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.ErrorDetails = $"Error : {ex.Message}";
            }
            return message;
        }
        public static async Task<IBusMessage> TryExecuteOperationWrapperAsync<T>(IBusMessage item, Func<T, CancellationToken, Task<object>> operation)
        {
            IBusMessage message = new ResponseMessage(item.Id, item.MessageType);
            try
            {
                CancellationTokenSource cts = TokenHelper.GetTokenSource(item.CreateDate, item.TTL, DateTime.UtcNow);

                T value = item.GetBody<T>();
                message.Body = await operation(value, cts.Token);

            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.ErrorDetails = $"Error : {ex.Message}";
            
            }
            return message;

        }

    }
}
