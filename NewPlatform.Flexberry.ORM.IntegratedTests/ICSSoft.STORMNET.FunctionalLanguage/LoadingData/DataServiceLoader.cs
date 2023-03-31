// namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage.LoadingData
// {
//    using System;
//    using System.Collections.Generic;
//    using System.Configuration;
//    using IIS.AMS02.Объекты;
//    using STORMNET.Business;

// /// <summary>
//    /// Класс для получения сервиса данных. TODO: заменить это всё на LocalDB.
//    /// </summary>
//    public class DataServiceLoader
//    {
//        /// <summary>
//        /// Необходимо для проверки валидности тестов.
//        /// Если кто-то добавит объекты в таблицу,
//        /// то результат вычитки по ограничению изменится и тесты будут не выполнены.
//        /// </summary>
//        public const int CountЗадержанный = 7;

// /// <summary>
//        /// Вспомогательная строка соединения.
//        /// </summary>
//        public const string NewCustomizationString = @"SERVER=storm\SQL2008R2;Trusted_connection=no;DATABASE=FilterTest;uid=CaseberryTestUser;pwd=testuser;";

// /// <summary>
//        /// Основная строка соединения.
//        /// </summary>
//        public static string BaseCustomizationString =
//            ConfigurationManager.AppSettings["CustomizationStrings"];

// /// <summary>
//        /// Инициализировать сервис данных для проверки загрузки данных и БД.
//        /// Настраивает на базу FilterTest.
//        /// </summary>
//        /// <returns>
//        /// Сконструированный сервис данных.
//        /// </returns>
//        public static IDataService InitializeDataSetvice()
//        {
//            return InitializeDataSetvice(NewCustomizationString);
//        }

// /// <summary>
//        /// Инициализировать сервис данных для проверки загрузки данных и БД.
//        /// </summary>
//        /// <param name="connectionString">
//        /// Строка подключения к SQL БД.
//        /// </param>
//        /// <returns>
//        /// Сконструированный сервис данных.
//        /// </returns>
//        private static IDataService InitializeDataSetvice(string connectionString)
//        {
//            var addAccessTypes = new Dictionary<Type, AccessType>
//            {
//                { typeof(Задержанный), AccessType.none },
//                { typeof(ПричинаЗадержания), AccessType.none },
//                { typeof(ИзъятоУЗадержанного), AccessType.none },
//                { typeof(ЛицоВЭпизоде), AccessType.none },
//                { typeof(ЦельПриездаЛица), AccessType.none },
//                { typeof(Преступник), AccessType.none },
//                { typeof(Потерпевший), AccessType.none },
//                { typeof(УголовноеДело), AccessType.none },
//                { typeof(ВидДокумента), AccessType.none },
//                { typeof(ЛицоУС), AccessType.none }
//            };
//            STORMNET.RightManager.SetAdditionalAccessTypes(addAccessTypes);

// var ds = new MSSQLDataService { CustomizationString = connectionString };
//            ds.UseCommandTimeout = true;
//            ds.CommandTimeout = 100000;
//            return ds;
//        }
//    }
// }
