namespace ICSSoft.STORMNET.Business
{
    /// <summary>
    /// Интерфейс для создания компонентов отслеживания выполнения бизнессзадач.
    /// </summary>
    public interface IBusinessTaskMonitor
    {
        /// <summary>
        /// Задача начала выполняться.
        /// </summary>
        /// <param name="TaskName">Имя задачи.</param>
        /// <returns>Некоторый идентификатор задачи в конкретном мониторе.</returns>
        object BeginTask(string TaskName);

        /// <summary>
        /// Задача начала выполняться.
        /// </summary>
        /// <param name="TaskName">Имя задачи.</param>
        /// <param name="ID">Некоторый идентификатор задачи в конкретном мониторе.</param>
        ///
        void BeginTask(string TaskName, object ID);

        /// <summary>
        /// Задача закончила выполняться.
        /// </summary>
        /// <param name="ID">Некоторый идентификатор задачи в конкретном мониторе.</param>
        void EndTask(object ID);

        /// <summary>
        /// ПодЗадача начала выполняться.
        /// </summary>
        /// <param name="SubTask">Имя подзадачи.</param>
        /// <param name="TaskID">Некоторый идентификатор задачи в конкретном мониторе.</param>
        /// <returns>Некоторый идентификатор подзадачи в конкретном мониторе.</returns>
        object BeginSubTask(string SubTask, object TaskID);

        /// <summary>
        /// ПодЗадача закончила выполняться.
        /// </summary>
        /// <param name="SubTaskID">Некоторый идентификатор подзадачи в конкретном мониторе.</param>
        void EndSubTask(object SubTaskID);
    }
}