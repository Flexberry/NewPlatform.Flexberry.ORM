namespace ICSSoft.STORMNET.Business
{
    //TODO: ivashkevich: устарело, следует использовать COP.Utils.ORM.IDataServiceWrapper
    /// <summary>
    /// Обертка для получения сервиса данных либо через <see cref="DataServiceProvider"/> либо явно указанный в этом экземпляре класса.
    /// Для поддержки этой возможности в классе наследнике необходимо определить конструктор с передачей экземпляра сервиса данных в базовый контруктор.
    /// </summary>
    public abstract class DataServiceWrapper
    {
        /// <summary>
        /// Явно указанный экземпляр сервиса данных с которым должен работать данный экземпляр класса.
        /// </summary>
        private readonly IDataService _customDataService;

        /// <summary>
        /// Сервис данных для работы в данном экземпляре класса.
        /// </summary>
        public IDataService DataService
        {
            get
            {
                return _customDataService ?? DataServiceProvider.DataService;
            }
        }

        /// <summary>
        /// Создание экземпляра класса по умолчанию. При этом сервис данных всегда будет браться из <see cref="DataServiceProvider"/>.
        /// </summary>
        protected DataServiceWrapper()
        {
        }

        /// <summary>
        /// Создание экземпляра класса с указанием сервиса данных для работы.
        /// </summary>
        /// <param name="dataService">Сервис данных с которым будет работать данный экземпляр класса.</param>
        protected DataServiceWrapper(IDataService dataService)
        {
            _customDataService = dataService;
        }
    }
}
