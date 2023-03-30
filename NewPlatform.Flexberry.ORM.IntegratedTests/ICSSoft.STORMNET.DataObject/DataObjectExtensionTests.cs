namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Linq.Expressions;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.IntegratedTests;

    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="DataObjectExtension"/>.
    /// </summary>
    public class DataObjectExtensionTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataObjectExtensionTests()
            : base("DOETest")
        {
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperty{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertyGenericTest1()
        {
            foreach (IDataService dataService in DataServices)
            {
                var client = new Клиент { Прописка = "Knowhere" };
                dataService.UpdateObject(client);

                Expression<Func<Клиент, object>> propertyExpression = x => x.Прописка;
                var view = new View { DefineClassType = typeof(Клиент) };
                view.AddProperty(Information.ExtractPropertyPath(propertyExpression));

                // Act.
                var loadedClient = PKHelper.CreateDataObject<Клиент>(client);
                dataService.LoadObject(view, loadedClient);

                Assert.True(client.CheckLoadedProperty(propertyExpression));
            }
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperty{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertyGenericMasterTest1()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                var country = new Страна { Название = "РФ" };
                var forest = new Лес { Страна = country };
                dataService.UpdateObject(forest);

                Expression<Func<Лес, object>> propertyExpression0 = x => x.Страна;
                Expression<Func<Лес, object>> propertyExpression = x => x.Страна.Название;
                var view = new View { DefineClassType = typeof(Лес) };
                view.AddProperty(Information.ExtractPropertyPath(propertyExpression));

                // Act.
                var loadedForest = PKHelper.CreateDataObject<Лес>(forest);
                dataService.LoadObject(view, loadedForest);

                // Assert.
                Assert.True(loadedForest.CheckLoadedProperty(propertyExpression0));
                Assert.False(loadedForest.CheckLoadedProperty(propertyExpression));
            }
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperties{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertiesGenericTest1()
        {
            foreach (IDataService dataService in DataServices)
            {
                var client = new Клиент { Прописка = "Knowhere" };
                dataService.UpdateObject(client);

                Expression<Func<Клиент, object>> propertyExpression = x => x.Прописка;
                var view = new View { DefineClassType = typeof(Клиент) };
                view.AddProperty(Information.ExtractPropertyPath(propertyExpression));

                // Act.
                var loadedClient = PKHelper.CreateDataObject<Клиент>(client);
                dataService.LoadObject(view, loadedClient);

                Assert.True(client.CheckLoadedProperties(propertyExpression));
            }
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperties{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertiesGenericTest2()
        {
            foreach (IDataService dataService in DataServices)
            {
                var client = new Клиент { Прописка = "Knowhere", ФИО = "Mr. Nobody" };
                dataService.UpdateObject(client);

                Expression<Func<Клиент, object>> propertyExpression = x => x.Прописка;
                Expression<Func<Клиент, object>> propertyExpression1 = x => x.ФИО;
                var view = new View { DefineClassType = typeof(Клиент) };
                view.AddProperty(Information.ExtractPropertyPath(propertyExpression));
                view.AddProperty(Information.ExtractPropertyPath(propertyExpression1));

                // Act.
                var loadedClient = PKHelper.CreateDataObject<Клиент>(client);
                dataService.LoadObject(view, loadedClient);

                Assert.True(client.CheckLoadedProperties(propertyExpression, propertyExpression1));
            }
        }
    }
}