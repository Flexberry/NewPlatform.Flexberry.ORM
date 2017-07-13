namespace ICSSoft.STORMNET.Business.Audit.HelpStructures
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Web.Configuration;
    using Security;

    /// <summary>
    /// Класс для получения данных из конфига.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Получение строки соединения из конфига.
        /// </summary>
        /// <param name="currentAppMode"> Текущий режим работы приложения (win или web). </param>
        /// <param name="connStringName"> Имя строки соединения. </param>
        /// <returns> Строка соединения. </returns>
        public static string GetConnectionString(AppMode currentAppMode, string connStringName)
        {
            var settingCollection =
                currentAppMode == AppMode.Web 
                ? WebConfigurationManager.ConnectionStrings 
                : ConfigurationManager.ConnectionStrings;
            return (from ConnectionStringSettings mi in settingCollection
                    where mi.Name == connStringName
                    select mi.ConnectionString).FirstOrDefault();
        }

        /// <summary>
        /// Получение настройки из конфига.
        /// </summary>
        /// <param name="currentAppMode"> Текущий режим работы приложения (win или web). </param>
        /// <param name="appSettingName"> Имя настройки. </param>
        /// <returns> Значение настройки. </returns>
        public static string GetAppSetting(AppMode currentAppMode, string appSettingName)
        {
            var settingCollection =
                currentAppMode == AppMode.Web
                ? WebConfigurationManager.AppSettings
                : ConfigurationManager.AppSettings;
            return (from mi in settingCollection.AllKeys
                    where mi == appSettingName
                    select settingCollection[mi]).FirstOrDefault();
        }

        /// <summary>
        /// На базе конфига пытаемся сконструировать сервис данных, который будет использоваться аудитом.
        /// При возможности формируется сервис данных, который не использует полномочий.
        /// </summary>
        /// <param name="currentAppMode"> Текущий режим работы приложения (win или web). </param>
        /// <param name="connStringName"> Имя строки соединения с БД аудита. </param>
        /// <returns> Сконструированный сервис аудита. </returns>
        public static IDataService ConstructProperAuditDataService(AppMode currentAppMode, string connStringName)
        {
            // Сначала пытаемся вычитать специфический для приложения тип сервиса данных.
            string dataServiceType =
                GetAppSetting(currentAppMode, connStringName + "_DSType");
            if (string.IsNullOrEmpty(dataServiceType))
            { 
                // Потом пытаемся вычитать тип сервиса данных для аудита по умолчанию (DefaultDsType).
                dataServiceType = GetAppSetting(currentAppMode, AuditConstants.DefaultDsTypeConfigName);
            }

            // Сначала пытаемся найти строку соединения с указанным именем.
            var dataServiceCustomizationString = GetConnectionString(currentAppMode, connStringName);

            Type dataServiceRealType =
                string.IsNullOrEmpty(dataServiceType) 
                    ? DataServiceProvider.DataService.GetType()
                    : Type.GetType(dataServiceType);

            if (dataServiceRealType == null || !(dataServiceRealType.IsSubclassOf(typeof(SQLDataService)) || dataServiceRealType == typeof(SQLDataService)))
            {
                throw new NotImplementedException("использование сервиса данных типа " + dataServiceType);
            }

            List<ConstructorInfo> foundConstructors = dataServiceRealType.GetConstructors()
                                                    .Where(x => x.GetParameters().Count() == 1 && x.GetParameters().All(y => y.ParameterType == typeof(ISecurityManager)))
                                                    .ToList();

            IDataService dataService = foundConstructors.Count == 1
                ? (IDataService)foundConstructors[0].Invoke(new object[] { new EmptySecurityManager() })
                : (IDataService)Activator.CreateInstance(dataServiceRealType);

            dataService.CustomizationString =
                string.IsNullOrEmpty(dataServiceCustomizationString) 
                    ? DataServiceProvider.DataService.CustomizationString
                    : dataServiceCustomizationString;
            return dataService;
        }
    }
}
