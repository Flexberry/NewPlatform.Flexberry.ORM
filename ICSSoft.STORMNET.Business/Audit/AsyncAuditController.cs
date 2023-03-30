namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Контроллер, отвечающий за асинхронность при работе аудита.
    /// </summary>
    public class AsyncAuditController
    {
        // TODO: Проверить и возможно организовать точку сбора исключений.
        // TODO: При реализации учесть возможность записи аудита некоторых классов через отдельный IAudit.

        #region Поля и свойства

        /// <summary>
        /// Непосредственно аудит, которому будут передаваться запросы.
        /// </summary>
        private IAudit _audit;

        /// <summary>
        /// Потоку пора остановиться.
        /// </summary>
        private bool threadNeedToStop = false;

        /// <summary>
        /// Поток по отправке данных для аудита.
        /// </summary>
        private Thread auditThread = null;

        /// <summary>
        /// Список заявок для потока отправки сообщений аудиту.
        /// </summary>
        private List<AuditParametersBase> threadQueue = null;

        /// <summary>
        /// Как долго спит поток, если нет заявок.
        /// </summary>
        private int? sleepTime = null;

        /// <summary>
        /// Непосредственно аудит, которому будут передаваться запросы.
        /// </summary>
        public IAudit Audit
        {
            private get
            {
                if (_audit == null)
                {
                    throw new Exception("В контроллере асинхронного аудита отсутствует инстанция IAudit");
                }

                return _audit;
            }

            set
            {
                _audit = value;
            }
        }

        /// <summary>
        /// Как долго спит поток, если нет заявок.
        /// </summary>
        private int SleepTime
        {
            get
            {
                if (sleepTime == null)
                { // TODO: сюда можно прикрепить вычитку из конфига
                    sleepTime = 5000;
                }

                return sleepTime.Value;
            }
        }

        /// <summary>
        /// Список заявок для потока отправки сообщений аудиту.
        /// </summary>
        private List<AuditParametersBase> ThreadQueue
        {
            get
            {
                return threadQueue ?? (threadQueue = new List<AuditParametersBase>());
            }
        }

        #endregion Поля и свойства

        /// <summary>
        /// Записываем операции в очередь на аудит.
        /// </summary>
        /// <param name="auditParametersBase">Параметры для их записи на аудит.</param>
        public void WriteAuditOperationAsync(AuditParametersBase auditParametersBase)
        {
            ThreadQueue.Add(auditParametersBase);
            StartAuditThread();
        }

        /// <summary>
        /// Если поток аудита не работает, то его запускают.
        /// </summary>
        private void StartAuditThread()
        {
            if (auditThread == null)
            {
                auditThread = new Thread(SendToAuditAsync);
                threadNeedToStop = false;
                auditThread.Start();
            }
        }

        /// <summary>
        /// На базе этого метода работает поток, который отправляет сообщения аудиту.
        /// </summary>
        private void SendToAuditAsync()
        {
            while (!threadNeedToStop)
            {
                while (ThreadQueue.Count > 0)
                {
                    AuditParametersBase auditParametersBase = ThreadQueue.First();
                    ThreadQueue.Remove(auditParametersBase);

                    // TODO: при асинхронной работе HttpContext.Current становится null, что приводит к ошибкам

                    var auditAddParameters = auditParametersBase as CommonAuditParameters;
                    if (auditAddParameters != null)
                    {
                        Audit.WriteCommonAuditOperation(auditAddParameters);
                    }

                    var auditRatifyParameters = auditParametersBase as RatificationAuditParameters;
                    if (auditRatifyParameters != null)
                    {
                        Audit.RatifyAuditOperation(auditRatifyParameters);
                    }

                    var auditApiParameters = auditParametersBase as CheckedCustomAuditParameters;
                    if (auditApiParameters != null)
                    {
                        Audit.WriteCustomAuditOperation(auditApiParameters);
                    }
                }

                Thread.Sleep(SleepTime);
            }
        }
    }
}
