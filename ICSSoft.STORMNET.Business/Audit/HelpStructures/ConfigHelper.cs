namespace ICSSoft.STORMNET.Business.Audit.HelpStructures
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using Security;

    /// <summary>
    /// Класс для получения данных из конфига.
    /// </summary>
    public static class ConfigHelper
    {
        private static readonly ConcurrentDictionary<string, IDataService> DataServiceCache =
            new ConcurrentDictionary<string, IDataService>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Получение строки соединения из конфига.
        /// </summary>
        /// <param name="connStringName"> Имя строки соединения. </param>
        /// <returns> Строка соединения. </returns>
        public static string GetConnectionString(string connStringName)
        {
            var settingCollection = ConfigurationManager.ConnectionStrings;
            return (from ConnectionStringSettings mi in settingCollection
                    where mi.Name == connStringName
                    select mi.ConnectionString).FirstOrDefault();
        }

        /// <summary>
        /// Получение настройки из конфига.
        /// </summary>
        /// <param name="appSettingName"> Имя настройки. </param>
        /// <returns> Значение настройки. </returns>
        public static string GetAppSetting(string appSettingName)
        {
            var settingCollection = ConfigurationManager.AppSettings;
            return (from mi in settingCollection.AllKeys
                    where mi == appSettingName
                    select settingCollection[mi]).FirstOrDefault();
        }

        /// <summary>
        /// На базе конфига пытаемся сконструировать сервис данных, который будет использоваться аудитом.
        /// При возможности формируется сервис данных, который не использует полномочий.
        /// </summary>
        /// <param name="connStringName"> Имя строки соединения с БД аудита. </param>
        /// <returns> Сконструированный сервис аудита. </returns>
        public static IDataService ConstructProperAuditDataService(string connStringName)
        {
            // Сначала пытаемся вычитать специфический для приложения тип сервиса данных.
            string dataServiceType = GetAppSetting(connStringName + "_DSType");
            if (string.IsNullOrEmpty(dataServiceType))
            {
                // Потом пытаемся вычитать тип сервиса данных для аудита по умолчанию (DefaultDsType).
                dataServiceType = GetAppSetting(AuditConstants.DefaultDsTypeConfigName);
            }

            // Сначала пытаемся найти строку соединения с указанным именем.
            var dataServiceCustomizationString = GetConnectionString(connStringName);

            var dataServiceReturnedByDataServiceProvider = DataServiceProvider.DataService;

            Type dataServiceRealType =
                string.IsNullOrEmpty(dataServiceType)
                    ? dataServiceReturnedByDataServiceProvider.GetType()
                    : Type.GetType(dataServiceType);

            if (dataServiceRealType == null || !(dataServiceRealType.IsSubclassOf(typeof(SQLDataService)) || dataServiceRealType == typeof(SQLDataService)))
            {
                throw new NotImplementedException("использование сервиса данных типа " + dataServiceType);
            }

            var realDataServiceCustomizationString = string.IsNullOrEmpty(dataServiceCustomizationString)
                ? dataServiceReturnedByDataServiceProvider.CustomizationString
                : dataServiceCustomizationString;

            var dataServiceCacheKey = $"{dataServiceRealType.FullName}_{realDataServiceCustomizationString}";

            return DataServiceCache.GetOrAdd(dataServiceCacheKey, _ =>
            {
                List<ConstructorInfo> foundConstructors = dataServiceRealType.GetConstructors()
                         .Where(x => x.GetParameters().Count() == 1 &&
                                     x.GetParameters().All(y => y.ParameterType == typeof(ISecurityManager)))
                         .ToList();

                IDataService dataService = foundConstructors.Count == 1
                    ? (IDataService)foundConstructors[0].Invoke(new object[] { new EmptySecurityManager() })
                    : (IDataService)Activator.CreateInstance(dataServiceRealType);

                dataService.CustomizationString = realDataServiceCustomizationString;
                return dataService;
            });
        }

        /// <summary>
        /// Очистка кеша сервисов данных аудита.
        /// </summary>
        public static void ClearAuditDataServiceCache()
        {
            DataServiceCache.Clear();
        }
    }
}
