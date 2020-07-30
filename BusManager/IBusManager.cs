using BusManager.Queue;

namespace BusManager
{
    /// <summary>
    /// менеджер управления очередями 
    /// </summary>
    public interface IBusManager
    {
        /// <summary>
        /// получение очереди по имени сервиса с которым данная очередь взаимодействет
        /// </summary>
        /// <param name="serviceName">наименование сервиса</param>
        /// <returns>интерфейс для взаимодействия с очередью</returns>
        IBusQueue GetQueue(string serviceName);

        /// <summary>
        /// проверка очереди на соединение , если соединение не инициализировано,
        /// то попытка создать его 
        /// </summary>
        /// <param name="serviceName">наименование сервиса</param>
        /// <returns>результат соединения</returns>
        bool IsInitQueue(string serviceName);

        /// <summary>
        /// метод проверяет инициализированны ли все очереди
        /// если нет , то инициализирует их 
        /// </summary>
        /// <returns>результат проверки/инициализации</returns>
        bool IsInitQueues();

        /// <summary>
        /// метод инициализирует все очереди из файла с настройками  
        /// </summary>
        void InitQueues();
        
        /// <summary>
        /// метод уничтожает все очереди и соединения
        /// </summary>
        void DestroyQueues();
        



    }
}
