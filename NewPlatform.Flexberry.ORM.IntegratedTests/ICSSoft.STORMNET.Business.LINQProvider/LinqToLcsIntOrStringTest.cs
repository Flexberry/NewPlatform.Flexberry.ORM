namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using NewPlatform.Flexberry.ORM.IntegratedTests;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Тесты на работу провайдера с ограничением вида:
    /// Where(DataObject.StringField == someString || DataObject.IntField.ToString() == someString).
    /// В качестве someString по задумке может прийти строка, гуид или целое число.
    /// Если приходит гуид, этот запрос падает с ошибкой приведения типов, потому, что генерируется запрос вида:
    /// WHERE ( ( "StringField " = N'7e30b4d0-5f62-494e-aa8b-4c8ffce49f78') OR ( "IntField" = N'7e30b4d0-5f62-494e-aa8b-4c8ffce49f78')).
    /// </summary>
    public class LinqToLcsIntOrStringTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public LinqToLcsIntOrStringTest()
            : base("LTest2")
        {
        }

        /// <summary>
        /// Тест работы ограничения, когда someString целое число преобразованное в строку,
        /// когда искомое значение находится в поле PoleInt.
        /// </summary>
        [Fact]
        public void TestSomeStringValueIntToStringPoleInt()
        {
            // Arrange.
            int intValue = 1;
            var someString = intValue.ToString();
            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService && typeof(SQLDataService).Assembly.ImageRuntimeVersion.StartsWith("v2"))
                {
                    // TODO: Исправить конвертацию для OracleDataService decimal в char, если используется System.Data.OracleClient (в Net3.5).
                    // Для версии Net4.0 и выше используется Oracle.ManagedDataAccess.Client, для которого исправление не требуется.
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var fullTypesMaster = new FullTypesMaster1() { PoleInt = intValue };
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);
                var view = FullTypesMaster1.Views.FullMasterView;
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleString == someString || x.PoleInt.ToString() == someString);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы ограничения, когда someString целое число преобразованное в строку,
        /// когда искомое значение находится в поле PoleString.
        /// </summary>
        [Fact]
        public void TestSomeStringValueIntToStringPoleString()
        {
            // Arrange.
            int intValue = 1;
            var someString = intValue.ToString();
            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService && typeof(SQLDataService).Assembly.ImageRuntimeVersion.StartsWith("v2"))
                {
                    // TODO: Исправить конвертацию для OracleDataService decimal в char, если используется System.Data.OracleClient (в Net3.5).
                    // Для версии Net4.0 и выше используется Oracle.ManagedDataAccess.Client, для которого исправление не требуется.
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var fullTypesMaster = new FullTypesMaster1() { PoleString = someString };
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);
                var view = FullTypesMaster1.Views.FullMasterView;
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleString == someString || x.PoleInt.ToString() == someString);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы ограничения, когда someString - Guid.
        /// </summary>
        [Fact(Skip = "Вернуть работоспособность теста после выполнения задачи 97733.")]
        public void TestSomeStringValueGuid()
        {
            // Arrange.
            var someString = Guid.NewGuid().ToString();
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;
                var fullTypesMaster = new FullTypesMaster1() { PoleString = someString };
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);
                var view = FullTypesMaster1.Views.FullMasterView;

                // Происходит падение теста так как генерируется запрос вида:
                // WHERE ( ( "StringField " = N'7e30b4d0-5f62-494e-aa8b-4c8ffce49f78')
                //      OR ( "IntField" = N'7e30b4d0-5f62-494e-aa8b-4c8ffce49f78')).
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleString == someString || x.PoleInt.ToString() == someString);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы ограничения, когда someString - строка.
        /// </summary>
        [Fact(Skip = "Вернуть работоспособность теста после выполнения задачи 97733.")]
        public void TestSomeStringValueString()
        {
            // Arrange.
            var someString = "Тестовая строка";
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;
                var fullTypesMaster = new FullTypesMaster1() { PoleString = someString };
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);
                var view = FullTypesMaster1.Views.FullMasterView;

                // Происходит падение теста так как генерируется запрос вида:
                // WHERE ( ( "PoleString" = N'Тестовая строка') OR ( "PoleInt" = N'Тестовая строка')).
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleString == someString || x.PoleInt.ToString() == someString);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы ограничения, когда someString - <see cref="Guid?"/> c непустым значением.
        /// </summary>
        [Fact(Skip = "Вернуть работоспособность теста после выполнения задачи 97733.")]
        public void TestSomeStringValueNullableGuidNotNull()
        {
            // Arrange.
            Guid? eqGuid = new Guid("{72FCA622-A01E-494C-BE1C-0821178594FB}");
            var someString = eqGuid.ToString();
            foreach (IDataService dataService in DataServices)
            {
                var ds = (SQLDataService)dataService;
                var fullTypesMaster = new FullTypesMaster1() { PoleString = someString };
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);
                var view = FullTypesMaster1.Views.FullMasterView;

                // Происходит падение теста так как генерируется запрос вида:
                // WHERE ( ( "PoleString" = N'72fca622-a01e-494c-be1c-0821178594fb')
                //      OR ( "PoleInt" = N'72fca622-a01e-494c-be1c-0821178594fb')).
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleString == someString || x.PoleInt.ToString() == someString);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы ограничения, когда someString - <see cref="Guid?"/> c пустым значением.
        /// </summary>
        [Fact]
        public void TestSomeStringValueNullableGuidNull()
        {
            // Arrange.
            Guid? eqGuid = null;
            var someString = eqGuid.ToString();
            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService && typeof(SQLDataService).Assembly.ImageRuntimeVersion.StartsWith("v2"))
                {
                    // TODO: Исправить конвертацию для OracleDataService decimal в char, если используется System.Data.OracleClient (в Net3.5).
                    // Для версии Net4.0 и выше используется Oracle.ManagedDataAccess.Client, для которого исправление не требуется.
                    continue;
                }

                var ds = (SQLDataService)dataService;
                var fullTypesMaster = new FullTypesMaster1() { PoleString = someString };
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);
                var view = FullTypesMaster1.Views.FullMasterView;
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleString == someString || x.PoleInt.ToString() == someString);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }
    }
}
