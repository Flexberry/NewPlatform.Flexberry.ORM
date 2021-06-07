namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    public class MultiThreadingTestTool
    {
        /// <summary>
        /// Инструмент для тестирования метода в многопоточном режиме.
        /// </summary>
        /// <param name="workerMethod">Метод, который будет работать в несколько потоков.</param>
        public MultiThreadingTestTool(ParameterizedThreadStart workerMethod)
        {
            WorkerMethod = workerMethod;
        }

        /// <summary>
        /// Имя параметра в коллекции значений, переданных в тестовый метод, указывающее на пользовательский параметр.
        /// </summary>
        public const string ParamNameSender = "sender";

        /// <summary>
        /// Имя параметра в коллекции значений, переданных в тестовый метод, указывающее на коллекцию исключений.
        /// </summary>
        public const string ParamNameExceptions = "ex";

        /// <summary>
        /// Имя параметра в коллекции значений, переданных в тестовый метод, указывающее на состояние работы потоков.
        /// </summary>
        public const string ParamNameWorking = "working";

        private const string LockObj = "CONST";
        private List<Thread> threads = new List<Thread>();

        /// <summary>
        /// Флаг, обозначающий необходимость работы потока. Если он стал false, значит надо прекратить выполнение операций. (Используем это для того, чтобы не обрывать выполнение потока жёстким методом Abort())
        /// </summary>
        public static bool Working { get; private set; } = true;

        /// <summary>
        /// Рабочие данные, которые можно выводить для отображения работы
        /// </summary>
        public static string WorkingData { get; set; } = string.Empty;

        /// <summary>
        /// Некоторый количественный показатель скорости работы, который можно выводить пользователю. Очищается после каждой операции снятия показаний.
        /// </summary>
        public static int WorkingSpeed { get; set; } = 0;

        /// <summary>
        /// Метод, который будет выполняться в несколько потоков.
        /// </summary>
        public ParameterizedThreadStart WorkerMethod = null;

        private ConcurrentDictionary<string, Exception> exceptions = new ConcurrentDictionary<string, Exception>();

        /// <summary>
        /// Получить коллекцию исключений.
        /// </summary>
        /// <returns>Исключения из потоков.</returns>
        public ExceptionInThreads GetExceptions()
        {
            if (exceptions.Count > 0)
            {
                return new ExceptionInThreads("Threads throw exceptions, see InnerExceptions property.", exceptions);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Запустить потоки.
        /// </summary>
        /// <param name="count">Количество потоков.</param>
        /// <param name="sender">Параметр, передаваемый в метод потока, в методе его можно будет достать из словаря <see cref="Dictionary{string, object}"/> по ключу <see cref="ParamNameSender"/>.</param>
        public void StartThreads(int count, object sender = null)
        {
            if (WorkerMethod == null)
            {
                throw new Exception("Укажите выполняющуюся функцию (WorkerMethod)");
            }

            Working = true;

            Thread[] threadsArray = new Thread[count];

            for (int i = 0; i < threadsArray.Length; i++)
            {
                threadsArray[i] = new Thread(WorkerMethod);
                threadsArray[i].Name = "Th_" + i.ToString();
                threads.Add(threadsArray[i]);
            }

            var dictionary = new Dictionary<string, object>();
            dictionary.Add(ParamNameSender, sender);
            dictionary.Add(ParamNameExceptions, exceptions);
            dictionary.Add(ParamNameWorking, Working);

            foreach (Thread thread in threadsArray)
            {
                thread.Start(dictionary);
            }

            foreach (Thread thread in threadsArray)
            {
                thread.Join();
            }
        }

        /// <summary>
        /// Остановить потоки через <see cref="Thread.Abort"/>.
        /// </summary>
        /// <param name="threadsStopNum">Количество потоков, которые нужно остановить.</param>
        public void StopThreads(int threadsStopNum)
        {
            for (int i = 0; i < threads.Count; i++)
            {
                if (threadsStopNum > 0)
                {
                    if (threads[i] != null && threads[i].IsAlive)
                    {
                        threads[i].Abort();
                        threadsStopNum--;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Получить количество живых потоков.
        /// </summary>
        /// <returns>Количество живых потоков.</returns>
        public int GetAliveThreadsCount()
        {
            int i = 0;
            for (int index = 0; index < threads.Count; index++)
            {
                Thread th = threads[index];
                if (th != null && th.IsAlive)
                {
                    i++;
                }
            }

            return i;
        }

        /// <summary>
        /// Исключения в потоках.
        /// </summary>
        public class ExceptionInThreads : Exception
        {
            /// <summary>
            /// Вложенные исключения.
            /// </summary>
            public ConcurrentDictionary<string, Exception> InnerExceptions { get; set; } = null;

            /// <summary>
            /// Создание исключения с вложением.
            /// </summary>
            /// <param name="exceptionMesage">Сообщение об исключении.</param>
            /// <param name="exceptions">Словарь исключений, возникших в потоках. В качестве ключа рекомендуется использовать имя потока.</param>
            public ExceptionInThreads(string exceptionMesage, ConcurrentDictionary<string, Exception> exceptions)
                : base(exceptionMesage, exceptions.Count > 0 ? exceptions.GetEnumerator().Current.Value : null)
            {
                InnerExceptions = exceptions;
            }
        }
    }
}
