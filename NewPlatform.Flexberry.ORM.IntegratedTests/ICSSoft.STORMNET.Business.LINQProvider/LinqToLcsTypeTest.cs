namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;

    using NewPlatform.Flexberry.ORM.IntegratedTests;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Проверка взаимодействия LinqProvider с Enumeration, Guid, 'Guid?' .
    /// </summary>
    public class LinqToLcsTypeTest : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public LinqToLcsTypeTest()
            : base("LTest2")
        {
        }

        /// <summary>
        /// Тест работы LinqProvider c полем типа <see cref="Guid"/>.
        /// </summary>
        [Fact]
        public void TestValueGuid()
        {
            // Arrange.
            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService && typeof(SQLDataService).Assembly.ImageRuntimeVersion.StartsWith("v2"))
                {
                    // TODO: Исправить конвертацию для OracleDataService decimal в char, если используется System.Data.OracleClient (в Net3.5).
                    // Для версии Net4.0 и выше используется Oracle.ManagedDataAccess.Client, для которого исправление не требуется.
                    continue;
                }

                var ds = (SQLDataService)dataService;

                // Контрольное значение.
                var testValue = Guid.NewGuid();

                // Создаём тестовый объект.
                var fullTypesMaster = new FullTypesMaster1() { PoleGuid = testValue };

                // Сохранение данных.
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMaster1.Views.FullMasterView;

                // Применение функции ограничения.
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleGuid == testValue);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы LinqProvider c полем типа <see cref="Guid?"/> c непустым значением.
        /// </summary>
        [Fact]
        public void TestValueNullableGuidNotNull()
        {
            // Arrange.
            foreach (IDataService dataService in DataServices)
            {
                // TODO: Fix OracleDataService error.
                if (dataService is OracleDataService)
                {
                    continue;
                }

                var ds = (SQLDataService)dataService;

                // Контрольное значение.
                Guid? testValue = Guid.NewGuid();

                // Создаём тестовый объект.
                var fullTypesMaster = new FullTypesMaster1() { PoleNullGuid = testValue };

                // Сохранение данных.
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMaster1.Views.FullMasterView;

                // Применение функции ограничения.
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleNullGuid == testValue).ToList();

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы LinqProvider c полем типа - <see cref="Guid?"/> c пустым значением.
        /// </summary>
        [Fact]
        public void TestValueNullableGuidNull()
        {
            // Arrange.
            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService && typeof(SQLDataService).Assembly.ImageRuntimeVersion.StartsWith("v2"))
                {
                    // TODO: Исправить конвертацию для OracleDataService decimal в char, если используется System.Data.OracleClient (в Net3.5).
                    // Для версии Net4.0 и выше используется Oracle.ManagedDataAccess.Client, для которого исправление не требуется.
                    continue;
                }

                var ds = (SQLDataService)dataService;

                // Контрольное значение.
                Guid? testValue = null;

                // Создаём тестовый объект.
                var fullTypesMaster = new FullTypesMaster1() { PoleNullGuid = testValue };

                // Сохранение данных.
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMaster1.Views.FullMasterView;

                // Применение функции ограничения.
                var query = ((SQLDataService)dataService)
                            .Query<FullTypesMaster1>(view)
                            .Where(x => x.PoleNullGuid == testValue);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }

        /// <summary>
        /// Тест работы LinqProvider c перечислением (первый вариант)
        /// В ограничение подставляется переменная testValue, содержащая значение перечисляемого типа PoleEnum.Attribute1.
        /// </summary>
        [Fact]
        public void TestValueEnum()
        {
            // Arrange.
            foreach (IDataService dataService in DataServices)
            {
                if (dataService is OracleDataService && typeof(SQLDataService).Assembly.ImageRuntimeVersion.StartsWith("v2"))
                {
                    // TODO: Исправить конвертацию для OracleDataService decimal в char, если используется System.Data.OracleClient (в Net3.5).
                    // Для версии Net4.0 и выше используется Oracle.ManagedDataAccess.Client, для которого исправление не требуется.
                    continue;
                }

                var ds = (SQLDataService)dataService;

                // Контрольное значение.
                PoleEnum testValue = PoleEnum.Attribute1;

                // Создаём тестовый объект.
                var fullTypesMaster = new FullTypesMaster1() { PoleEnum = testValue };

                // Сохранение данных.
                var updateObjectsArray = new DataObject[] { fullTypesMaster };
                ds.UpdateObjects(ref updateObjectsArray);

                // Выбор представления.
                var view = FullTypesMaster1.Views.FullMasterView;

                // Применение функции ограничения.
                var query = ds.Query<FullTypesMaster1>(view)
                              .Where(x => x.PoleEnum == testValue);

                // Act.
                FullTypesMaster1 result = query.FirstOrDefault();

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(fullTypesMaster.__PrimaryKey, result.__PrimaryKey);
            }
        }
    }
}


