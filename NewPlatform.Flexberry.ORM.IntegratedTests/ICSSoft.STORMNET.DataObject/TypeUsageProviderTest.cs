namespace ICSSoft.STORMNET.Tests.TestClasses.DataObject
{
    using System;

    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Тестовый класс для TypeUsageProvider.
    /// </summary>
    public class TypeUsageProviderTest
    {
        /// <summary>
        /// Проверка получения используемых типов.
        /// </summary>
        [Fact]

        public void TypeUsageProviderGetUsageTypesTest()
        {
            TypeUsage typeUsage = new TypeUsage();

            ICSSoft.STORMNET.DataObject dataObject = new TypeUsageProviderTestClass();

            var result = typeUsage.GetUsageTypes(
                dataObject.GetType(), Information.ExtractPropertyPath<TypeUsageProviderTestClass>(x => x.SomeNotStoredObjectProperty));
            Assert.Equal("String", result[0].Name);
            Assert.Equal("Int32", result[1].Name);
            Assert.True(2 == result.Length, "Количество используемых типов.");
        }

        /// <summary>
        /// Проверка получения используемых типов у DetailArray.
        /// </summary>
        [Fact]

        public void TypeUsageProviderGetUsageTypesTest1()
        {
            TypeUsage typeUsage = new TypeUsage();

            var dataObject = new TypeUsageProviderTestClass();

            var result = typeUsage.GetUsageTypes(
                dataObject.GetType(),
                Information.ExtractPropertyPath<TypeUsageProviderTestClass>(x => x.CombinedTypesUsageProviderTestClass));
            Assert.Equal("CombinedTypesUsageProviderTestClass", result[0].Name);
            Assert.True(1 == result.Length, "Количество используемых типов.");
        }

        /// <summary>
        /// Проверка установки и добавления используемых типов.
        /// </summary>
        [Fact]

        public void TypeUsageProviderSetAndAddUseTypesTest()
        {
            TypeUsage typeUsage = new TypeUsage();

            var dataObject = new TypeUsageProviderTestClass();
            string propertyName =
                Information.ExtractPropertyPath<TypeUsageProviderTestClass>(x => x.SomeNotStoredObjectProperty);

            // Получение используемых типов.
            var result = typeUsage.GetUsageTypes(dataObject.GetType(), propertyName);
            Assert.True(2 == result.Length, "Количество используемых типов.");

            // Установка новых используемых типов.
            typeUsage.SetUsageTypes(dataObject.GetType(), propertyName, new Type[] { typeof(Boolean) });
            result = typeUsage.GetUsageTypes(dataObject.GetType(), propertyName);
            Assert.Equal("Boolean", result[0].Name);
            Assert.True(1 == result.Length, "Количество используемых типов.");

            // Добавление используемого типа.
            typeUsage.AddUsageTypes(dataObject.GetType(), propertyName, new Type[] { typeof(Char) });
            result = typeUsage.GetUsageTypes(dataObject.GetType(), propertyName);
            Assert.Equal("Boolean", result[0].Name);
            Assert.Equal("Char", result[1].Name);
            Assert.True(2 == result.Length, "Количество используемых типов.");
        }

        /// <summary>
        /// Проверка получения комбинируемых типов.
        /// </summary>
        [Fact]

        public void TypeUsageProviderGetCombinedTypeUsageTest()
        {
            TypeUsage typeUsage = new TypeUsage();

            var dataObject = new TypeUsageProviderTestClass();

            var result = typeUsage.GetCombinedTypeUsage(dataObject.GetType(), "DataObjectForTest.Name");
            Assert.Equal("String", result[0].Name);
            Assert.True(1 == result.Length, "Количество используемых типов.");
        }

        /// <summary>
        /// Проверка получения сложных комбинированных типов.
        /// </summary>
        [Fact]

        public void TypeUsageProviderGetCombinedTypeUsage1Test()
        {
            var typeUsage = new TypeUsage();

            var dataObject = new CombinedTypesUsageProviderTestClass();

            /*
             * У типа CombinedTypesUsageProviderTestClass на свойство TypeUsageProviderTestClass
             * через TypeUsage завязано 2 типа:
             * "NewPlatform.Flexberry.ORM.Tests.TypeUsageProviderTestClass" и "NewPlatform.Flexberry.ORM.Tests.TypeUsageProviderTestClassChildClass".
             * У каждого типа есть свойство "SomeNotStoredObjectProperty".
             * У первого типа через TypeUsage на данное свойство повешены типы String и Int32,
             * у второго - ничего, поэтому берётся исходный тип свойства Object.
             */
            var result = typeUsage.GetCombinedTypeUsage(
                dataObject.GetType(),
                Information.ExtractPropertyPath<CombinedTypesUsageProviderTestClass>(x => x.TypeUsageProviderTestClass.SomeNotStoredObjectProperty));

            Assert.Equal("String", result[0].Name);
            Assert.Equal("Int32", result[1].Name);
            Assert.Equal("Object", result[2].Name);
            Assert.True(3 == result.Length, "Количество используемых типов.");
        }
    }
}
