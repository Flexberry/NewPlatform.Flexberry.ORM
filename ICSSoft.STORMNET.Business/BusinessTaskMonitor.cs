namespace ICSSoft.STORMNET.Business
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Провайдер текущего монитора выполнения задач.
    /// </summary>
    public class BusinessTaskMonitor
    {
        private BusinessTaskMonitor()
        {
        }

        static BusinessTaskMonitor()
        {
            try
            {
                string taskCompName = ConfigurationManager.AppSettings["BusinessTaskMonitorType"];
                if (taskCompName != null && taskCompName != string.Empty)
                {
                    taskMonitor = (IBusinessTaskMonitor)Activator.CreateInstance(Type.GetType(taskCompName));
                }
            }
            catch { }
        }

        private static IBusinessTaskMonitor taskMonitor;

        /// <summary>
        /// текущий монитор.
        /// </summary>
        public static IBusinessTaskMonitor TaskMonitor
        {
            get
            {
                return taskMonitor;
            }

            set
            {
                taskMonitor = value;
            }
        }

        /// <summary>
        /// Задача начала выполняться.
        /// </summary>
        /// <param name="TaskName">имя задачи.</param>
        /// <returns>некоторый идентификатор задачи в конкретном мониторе.</returns>
        public static object BeginTask(string TaskName)
        {
            if (taskMonitor != null)
            {
                return taskMonitor.BeginTask(TaskName);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Задача начала выполняться.
        /// </summary>
        /// <param name="TaskName">имя задачи.</param>
        /// <param name="ID">некоторый идентификатор задачи в конкретном мониторе.</param>
        public static void BeginTask(string TaskName, object ID)
        {
            if (ID != null && taskMonitor != null)
            {
                taskMonitor.BeginTask(TaskName, ID);
            }
        }

        /// <summary>
        /// Задача закончила выполняться.
        /// </summary>
        /// <param name="ID">некоторый идентификатор задачи в конкретном мониторе.</param>
        public static void EndTask(object ID)
        {
            if (taskMonitor != null)
            {
                taskMonitor.EndTask(ID);
            }
        }

        /// <summary>
        /// ПодЗадача начала выполняться.
        /// </summary>
        /// <param name="SubTask">имя подзадачи.</param>
        /// <param name="TaskID">некоторый идентификатор задачи в конкретном мониторе.</param>
        /// <returns>некоторый идентификатор подзадачи в конкретном мониторе.</returns>
        public static object BeginSubTask(string SubTask, object TaskID)
        {
            if (taskMonitor != null)
            {
                return taskMonitor.BeginSubTask(SubTask, TaskID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ПодЗадача закончила выполняться.
        /// </summary>
        /// <param name="SubTaskID">некоторый идентификатор подзадачи в конкретном мониторе.</param>
        public static void EndSubTask(object SubTaskID)
        {
            if (taskMonitor != null)
            {
                taskMonitor.EndSubTask(SubTaskID);
            }
        }
    }
}
