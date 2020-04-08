namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.UserDataTypes;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты загрузки нехранимых свойств.
    /// </summary>
    public class NotStoredPropertyLoadingTests : BaseIntegratedTest
    {
        public NotStoredPropertyLoadingTests()
            : base("NotStrPr")
        {
        }

        /// <summary>
        /// Тест проверяет, что нехранимое свойство считается загруженным для всех сервисов данных.
        /// У свойства <see cref="StoredClass.NotStoredProperty" /> присутствует атрибут <see cref="DataServiceExpressionAttribute" /> для всех типов сервиса данных.
        /// </summary>
        [Fact]
        public void TestLoadingNotStoredPropertyWithAllExpressions()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var guidBreed = new Guid("a1a31995-4322-47a8-8128-da251826b958");
                var guidCat = new Guid("89136856-459e-418d-a3be-547a60192517");
                var breed = new Порода { __PrimaryKey = guidBreed, Название = "Чеширская" };
                var cat = new Кошка { __PrimaryKey = guidCat, ДатаРождения = NullableDateTime.UtcNow, Тип = ТипКошки.Домашняя, Порода = breed, Кличка = "Мурзик" };
                dataService.UpdateObject(cat);

                var viewCat = new View { DefineClassType = typeof(Кошка) };
                viewCat.AddProperties(
                    Information.ExtractPropertyPath<Кошка>(c => c.Кличка),
                    Information.ExtractPropertyPath<Кошка>(c => c.КошкаСтрокой));

                // Act.
                var loadedCat = PKHelper.CreateDataObject<Кошка>(cat);
                dataService.LoadObject(viewCat, loadedCat);

                // Assert.
                Assert.Equal(3, loadedCat.GetLoadedProperties().Length);
                Assert.True(loadedCat.CheckLoadedProperty(c => c.КошкаСтрокой));
            }
        }

        /// <summary>
        /// Тест проверяет, что нехранимое свойство считается не загруженным для всех сервисов данных.
        /// Данное поведение обусловлено отсутствием <see cref="DataServiceExpressionAttribute" /> у свойства <see cref="StoredClass.NotStoredProperty" />.
        /// </summary>
        [Fact]
        public void TestLoadingNotStoredPropertyWithNoExpression()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var guidStoredClass = new Guid("89136856-459e-418d-a3be-547a60192517");
                var storedClass = new StoredClass { __PrimaryKey = guidStoredClass, StoredProperty = "Some data" };
                dataService.UpdateObject(storedClass);

                var viewStoredClass = new View { DefineClassType = typeof(StoredClass) };
                viewStoredClass.AddProperties(
                    Information.ExtractPropertyPath<StoredClass>(c => c.StoredProperty),
                    Information.ExtractPropertyPath<StoredClass>(c => c.NotStoredProperty));

                // Act.
                var loadedStoredClass = PKHelper.CreateDataObject<StoredClass>(storedClass);
                dataService.LoadObject(viewStoredClass, loadedStoredClass);

                // Assert.
                Assert.Equal(3, loadedStoredClass.GetLoadedProperties().Length);
                Assert.True(loadedStoredClass.CheckLoadedProperty(x => x.NotStoredProperty));
            }
        }

        /// <summary>
        /// Тест проверяет, что нехранимое свойство считается загруженным, только если добавлено DSE для соответствующего сервиса данных.
        /// <see cref="DataServiceExpressionAttribute" /> определен <see cref="Медведь.ВычислимоеПоле" /> только для одного типа сервиса данных.
        /// </summary>
        [Fact]
        public void TestLoadingNotStoredProperty()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var guidBear = new Guid("89136856-459e-418d-a3be-547a60192517");
                var bear = new Медведь { __PrimaryKey = guidBear, ПорядковыйНомер = 322, Вес = 500 };
                dataService.UpdateObject(bear);

                var viewBear = new View { DefineClassType = typeof(Медведь) };
                viewBear.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(b => b.ПорядковыйНомер),
                    Information.ExtractPropertyPath<Медведь>(b => b.Вес),
                    Information.ExtractPropertyPath<Медведь>(b => b.ВычислимоеПоле));

                // Act.
                var loadedBear = PKHelper.CreateDataObject<Медведь>(bear);
                dataService.LoadObject(viewBear, loadedBear);

                // Assert.
                Assert.Equal(4, loadedBear.GetLoadedProperties().Length);
                Assert.True(loadedBear.CheckLoadedProperty(x => x.ВычислимоеПоле));
            }
        }
    }
}
