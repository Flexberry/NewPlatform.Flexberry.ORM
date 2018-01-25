namespace ICSSoft.STORMNET.Business.Audit
{
    using System;
#if DNX4
    using System.ServiceModel;
    using ICSSoft.STORMNET.Business.AuditWcfServiceLibrary;
#endif
    /// <summary>
    /// Класс для организации доcтупа к сервису аудита через wcf
    /// </summary>
    public class RemoteAuditController
    { 
#if DNX4
        /// <summary>
        /// Фабрика каналов для связи.
        /// </summary>
        private ChannelFactory<IAuditWcfService> _channelFactory = null;

        /// <summary>
        /// Экземпляр wcf-сервиса аудита.
        /// </summary>
        private IAuditWcfService _service = null;

        /// <summary>
        /// Закешированное значение адреса, где располагается сервис 
        /// (используется, чтобы не пересоздавать подключение к сервису).
        /// </summary>
        private string _cashedUri = string.Empty;

        /// <summary>
        /// Провести аудит операции
        /// </summary>
        /// <param name="auditParameters">Параметры аудита</param>
        /// <param name="uri">Адрес, где располагается сервис аудита</param>
        /// <returns>Guid записи аудита</returns>
        public Guid? WriteAuditOperation(AuditParameters auditParameters, string uri)
        { // Единая точка ловли исключений - соответствующий метод в AuditService.
            Guid? auditOperationId = null;
            CreateWcfService(uri);

            if (auditParameters is CommonAuditParameters)
            {
                auditOperationId = _service.WriteCommonAuditOperation((CommonAuditParameters)auditParameters);
                return auditOperationId;
            }

            if (auditParameters is CheckedCustomAuditParameters)
            {
                auditOperationId = _service.WriteCustomAuditOperation((CheckedCustomAuditParameters)auditParameters);
                return auditOperationId;
            }

            throw new NotSupportedException("Передан неподдерживаемый тип параметров аудита");
        }

        /// <summary>
        /// Провести подтверждение операции аудита
        /// </summary>
        /// <param name="ratificationAuditParameters">
        /// Параметры аудита
        /// </param>
        /// <param name="uri">
        /// Адрес, где располагается сервис аудита
        /// </param>
        /// <returns>
        /// True, если подтверждение выполнено успешно
        /// </returns>
        public bool RatifyAuditOperation(RatificationAuditParameters ratificationAuditParameters, string uri)
        { // Единая точка ловли исключений - соответствующий метод в AuditService.
            CreateWcfService(uri);
            _service.RatifyAuditOperation(ratificationAuditParameters);
            return true;
        }

        /// <summary>
        /// Создание экземпляра соединения с wcf-сервисом
        /// </summary>
        /// <param name="uri">
        /// Адрес, где располагается wcf-сервис
        /// </param>
        private void CreateWcfService(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri", "Передан пустой адрес сервиса аудита");
            }

            if (_cashedUri != uri)
            {
                var binding = new BasicHttpBinding
                {
                    CloseTimeout = new TimeSpan(0, 10, 0),
                    OpenTimeout = new TimeSpan(0, 10, 0),
                    ReceiveTimeout = new TimeSpan(0, 10, 0),
                    SendTimeout = new TimeSpan(0, 10, 0)
                };

                var address = new EndpointAddress(uri);
                _channelFactory = new ChannelFactory<IAuditWcfService>(binding, address);
                _cashedUri = uri;
                _service = _channelFactory.CreateChannel();
            }
        }
#endif
}
}
