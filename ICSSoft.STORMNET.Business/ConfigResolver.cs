namespace ICSSoft.STORMNET.Business
{

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Business.Audit.HelpStructures;

    /// <summary>
    /// Класс, реализующий разрешение свойств классов на основе данных из файла конфигурации для приложений на базе .NET framework.
    /// </summary>
    public class ConfigResolver : IConfigResolver
    {
        /// <inheritdoc cref="IConfigResolver"/>
        public string ResolveConnectionString(string connStringName)
        {
            // Определяем, в каком режиме работает приложение - Web или Win.
            var appMode = DataServiceProvider.IsWebApp ? AppMode.Web : AppMode.Win;

            return ConfigHelper.GetConnectionString(appMode, connStringName);
        }
    }
}
