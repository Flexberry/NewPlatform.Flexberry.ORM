namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;

    using ICSSoft.STORMNET;

    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="DataObjectExtension"/>.
    /// </summary>
    public class DataObjectExtensionTests
    {
        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperty{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertyGenericTest1()
        {
            var client = new Клиент();

            // Мы только создали объект, он не является загруженным.
            Assert.False(client.CheckLoadedProperty(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperty{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertyGenericTest2()
        {
            var client = new Клиент { Прописка = "Knowhere" };
            client.SetLoadedProperties(nameof(Клиент.Прописка));

            Assert.True(client.CheckLoadedProperty(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperties{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertiesGenericTest1()
        {
            var client = new Клиент();

            // Мы только создали объект, он не является загруженным.
            Assert.False(client.CheckLoadedProperties(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperties{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertiesGenericTest2()
        {
            var client = new Клиент { Прописка = "Knowhere" };
            client.SetLoadedProperties(nameof(Клиент.Прописка));

            Assert.True(client.CheckLoadedProperties(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperties{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertiesGenericTest3()
        {
            var client = new Клиент { Прописка = "Knowhere", ФИО = "Mr. Nobody" };
            client.SetLoadedProperties(nameof(Клиент.Прописка));

            Assert.False(client.CheckLoadedProperties(x => x.Прописка, x => x.ФИО));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.CheckLoadedProperties{T}" />.
        /// </summary>
        [Fact]
        public void CheckLoadedPropertiesGenericTest4()
        {
            var client = new Клиент { Прописка = "Knowhere", ФИО = "Mr. Nobody" };
            client.SetLoadedProperties(nameof(Клиент.Прописка), nameof(Клиент.ФИО));

            Assert.True(client.CheckLoadedProperties(x => x.Прописка, x => x.ФИО));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.IsAlteredProperty{T}" />.
        /// </summary>
        [Fact]
        public void IsAlteredPropertyGenericTest1()
        {
            var client = new Клиент();

            // Мы только создали объект, все свойства Altered.
            Assert.True(client.IsAlteredProperty(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.IsAlteredProperty{T}" />.
        /// </summary>
        [Fact]
        public void IsAlteredPropertyGenericTest2()
        {
            var client = new Клиент();
            client.SetExistObjectPrimaryKey(Guid.NewGuid());
            client.InitDataCopy();

            Assert.False(client.IsAlteredProperty(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.IsAlteredProperties{T}" />.
        /// </summary>
        [Fact]
        public void IsAlteredPropertiesGenericTest1()
        {
            var client = new Клиент();

            // Мы только создали объект, все свойства Altered.
            Assert.True(client.IsAlteredProperties(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.IsAlteredProperties{T}" />.
        /// </summary>
        [Fact]
        public void IsAlteredPropertiesGenericTest2()
        {
            var client = new Клиент();
            client.SetExistObjectPrimaryKey(Guid.NewGuid());
            client.InitDataCopy();

            Assert.False(client.IsAlteredProperties(x => x.Прописка));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.IsAlteredProperties{T}" />.
        /// </summary>
        [Fact]
        public void IsAlteredPropertiesGenericTest3()
        {
            var client = new Клиент();
            client.SetExistObjectPrimaryKey(Guid.NewGuid());
            client.InitDataCopy();
            client.ФИО = "Mr. Nobody";

            Assert.True(client.IsAlteredProperties(x => x.Прописка, x => x.ФИО));
        }

        /// <summary>
        /// Тест Linq-расширения метода <see cref="DataObjectExtension.IsAlteredProperties{T}" />.
        /// </summary>
        [Fact]
        public void IsAlteredPropertiesGenericTest4()
        {
            var client = new Клиент();
            client.SetExistObjectPrimaryKey(Guid.NewGuid());
            client.InitDataCopy();

            Assert.False(client.IsAlteredProperties(x => x.Прописка, x => x.ФИО));
        }
    }
}