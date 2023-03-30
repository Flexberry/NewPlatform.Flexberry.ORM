namespace NewPlatform.Flexberry.ORM.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using ICSSoft.STORMNET.Business.Interfaces;
    using ICSSoft.STORMNET.Exceptions;

    using Xunit;

    using NewPlatform.Flexberry.ORM.Tests;
    using ICSSoft.STORMNET;

    /// <summary>
    /// Класс для тестирования функциональности класса <see cref="InterfaceBusinessServer" />.
    /// </summary>
    public class InterfaceTests
    {
        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.ReferencePropertyInfo.FormList" />:  Формируем из
        /// предоставленной сборки соответствия между типом и списком свойства, которыми он ссылается на заданный мастеровой
        /// тип.
        /// Передаём тип, на который в данной сборке никто не ссылается как на мастера.
        /// </summary>
        [Fact]
        public void TestReferencePropertyInfoFormList()
        {
            var resultList = InterfaceBusinessServer.ReferencePropertyInfo.FormList(typeof(Territory2).Assembly, typeof(Apparatus2));
            Assert.Equal(0, resultList.Count);
        }

        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.ReferencePropertyInfo.FormList" />:  Формируем из
        /// предоставленной сборки соответствия между типом и списком свойства, которыми он ссылается на заданный мастеровой
        /// тип.
        /// Передаём тип, на который в данной сборке некоторые классы ссылаются как на мастера (наследование при этом учитывать
        /// нужно).
        /// </summary>
        [Fact]
        public void TestReferencePropertyInfoFormList2()
        {
            var resultList = InterfaceBusinessServer.ReferencePropertyInfo.FormList(typeof(Territory2).Assembly, typeof(Country2));
            Assert.Equal(4, resultList.Count);

            // Apparatus2.
            var apparatusType = resultList.FirstOrDefault(x => x.TypeWithReference == typeof(Apparatus2));
            Assert.NotNull(apparatusType);
            Assert.Equal(2, apparatusType.ReferenceProperties.Count);
            Assert.False(string.IsNullOrEmpty(apparatusType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Apparatus2>(properties => properties.Exporter))));
            Assert.False(string.IsNullOrEmpty(apparatusType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Apparatus2>(properties => properties.Maker))));

            // Adress2.
            var adressType = resultList.FirstOrDefault(x => x.TypeWithReference == typeof(Adress2));
            Assert.NotNull(adressType);
            Assert.Equal(1, adressType.ReferenceProperties.Count);
            Assert.False(string.IsNullOrEmpty(adressType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Adress2>(properties => properties.Country))));

            // Place2.
            var placeType = resultList.FirstOrDefault(x => x.TypeWithReference == typeof(Place2));
            Assert.NotNull(placeType);
            Assert.Equal(2, placeType.ReferenceProperties.Count);
            Assert.False(string.IsNullOrEmpty(placeType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Place2>(properties => properties.TodayTerritory))));
            Assert.False(string.IsNullOrEmpty(placeType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Place2>(properties => properties.TomorrowTeritory))));

            // Human2.
            var humanType = resultList.FirstOrDefault(x => x.TypeWithReference == typeof(Human2));
            Assert.NotNull(humanType);
            Assert.Equal(1, humanType.ReferenceProperties.Count);
            Assert.False(string.IsNullOrEmpty(humanType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Human2>(properties => properties.TodayHome))));
        }

        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.ReferencePropertyInfo.FormList" />:  Формируем из
        /// предоставленной сборки соответствия между типом и списком свойства, которыми он ссылается на заданный мастеровой
        /// тип.
        /// Передаём тип, на который в данной сборке некоторые классы ссылаются как на мастера (наследование при этом учитывать
        /// не нужно).
        /// </summary>
        [Fact]
        public void TestReferencePropertyInfoFormList3()
        {
            var resultList = InterfaceBusinessServer.ReferencePropertyInfo.FormList(typeof(Territory2).Assembly, typeof(Territory2));
            Assert.Equal(2, resultList.Count);

            // Place2.
            var placeType = resultList.FirstOrDefault(x => x.TypeWithReference == typeof(Place2));
            Assert.NotNull(placeType);
            Assert.Equal(2, placeType.ReferenceProperties.Count);
            Assert.False(string.IsNullOrEmpty(placeType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Place2>(properties => properties.TodayTerritory))));
            Assert.False(string.IsNullOrEmpty(placeType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Place2>(properties => properties.TomorrowTeritory))));

            // Human2.
            var humanType = resultList.FirstOrDefault(x => x.TypeWithReference == typeof(Human2));
            Assert.NotNull(humanType);
            Assert.Equal(1, humanType.ReferenceProperties.Count);
            Assert.False(string.IsNullOrEmpty(humanType.ReferenceProperties.FirstOrDefault(x => x == Information.ExtractPropertyPath<Human2>(properties => properties.TodayHome))));
        }

        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.FormViewOnReferencePropertyInfo" />:  Формируем представление,
        /// основываясь на информации о типе и необходимых в представлении свойств.
        /// </summary>
        [Fact]
        public void TestFormViewOnReferencePropertyInfo()
        {
            var someReferencePropertyInfo = InterfaceBusinessServer.ReferencePropertyInfo.FormList(typeof(Territory2).Assembly, typeof(Territory2)).FirstOrDefault();
            Assert.NotNull(someReferencePropertyInfo);
            var resultView = InterfaceBusinessServer.FormViewOnReferencePropertyInfo(someReferencePropertyInfo);
            Assert.Equal(someReferencePropertyInfo.TypeWithReference, resultView.DefineClassType);
            Assert.Equal(someReferencePropertyInfo.ReferenceProperties.Count, resultView.Properties.Count());
            foreach (var currentProperty in someReferencePropertyInfo.ReferenceProperties)
            {
                Assert.False(string.IsNullOrEmpty(resultView.Properties.Where(x => x.Name == currentProperty).Select(x => x.Name).FirstOrDefault()));
            }
        }

        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.FormLimitFunctionOnReferencePropertyInfo" />:  Формируем функцию
        /// ограничения, с помощью которой можно выявить все объекты, которые имеют мастеровую ссылку на интересующий объект.
        /// В списке несколько свойств.
        /// </summary>
        [Fact]
        public void TestFormLimitFunctionOnReferencePropertyInfo()
        {
            var territory = new Territory2();
            var propertyList = new List<string> { "Property1", "Property2" };
            var resultFunction = InterfaceBusinessServer.FormLimitFunctionOnReferencePropertyInfo(propertyList, territory);
            Assert.Equal(string.Format("OR ( = ( {1} {0} )  = ( {2} {0} ) )", territory.__PrimaryKey, propertyList[0], propertyList[1]), resultFunction.ToString());
        }

        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.FormLimitFunctionOnReferencePropertyInfo" />:  Формируем функцию
        /// ограничения, с помощью которой можно выявить все объекты, которые имеют мастеровую ссылку на интересующий объект.
        /// В списке одно свойство.
        /// </summary>
        [Fact]
        public void TestFormLimitFunctionOnReferencePropertyInfo2()
        {
            var territory = new Territory2();
            var propertyList = new List<string> { "Property1" };
            var resultFunction = InterfaceBusinessServer.FormLimitFunctionOnReferencePropertyInfo(propertyList, territory);
            Assert.Equal(string.Format("OR ( = ( {1} {0} ) )", territory.__PrimaryKey, propertyList[0]), resultFunction.ToString());
        }

        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.NullifyMasterReferences" />:  Вместо ссылки на удаляемого
        /// мастера проставляем <c>null</c> в соответствующие свойства объектов.
        /// Передаём разные объекты. Ни один из них не имеет ссылки на удаляемого мастера по свойству с атрибутом NotNull.
        /// </summary>
        [Fact]
        public void TestNullifyMasterReferences()
        {
            var resultList = InterfaceBusinessServer.ReferencePropertyInfo.FormList(typeof(Country2).Assembly, typeof(Country2));
            var masterObject = new Country2();
            var masterObjectCopy = new Country2 { __PrimaryKey = masterObject.__PrimaryKey };
            var otherCountry = new Country2();
            var referencingDataObjects = new List<DataObject>
            {
                new Adress2 { Country = otherCountry },
                new Apparatus2 { Maker = masterObject, Exporter = otherCountry },
                new Human2 { TodayHome = masterObjectCopy },
                new Place2 { TodayTerritory = masterObject, TomorrowTeritory = otherCountry },
                new Apparatus2 { Maker = otherCountry, Exporter = otherCountry },
                new Human2 { TodayHome = otherCountry },
                new Place2 { TodayTerritory = otherCountry, TomorrowTeritory = otherCountry },
            };

            InterfaceBusinessServer.NullifyMasterReferences(masterObject, referencingDataObjects, resultList);
            Assert.Null(((Apparatus2)referencingDataObjects[1]).Maker);
            Assert.Null(((Human2)referencingDataObjects[2]).TodayHome);
            Assert.Null(((Place2)referencingDataObjects[3]).TodayTerritory);
            Assert.NotNull(((Apparatus2)referencingDataObjects[4]).Maker);
            Assert.NotNull(((Human2)referencingDataObjects[5]).TodayHome);
            Assert.NotNull(((Place2)referencingDataObjects[6]).TodayTerritory);
        }

        /// <summary>
        /// Тестируем метод <see cref="InterfaceBusinessServer.NullifyMasterReferences" />:  Вместо ссылки на удаляемого
        /// мастера проставляем <c>null</c> в соответствующие свойства объектов.
        /// Передаём объект, который имеет ссылки на удаляемого мастера по свойству с атрибутом NotNull.
        /// </summary>
        [Fact]
        public void TestNullifyMasterReferences2()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var resultList = InterfaceBusinessServer.ReferencePropertyInfo.FormList(typeof(Country2).Assembly, typeof(Country2));
                var masterObject = new Country2();
                var referencingDataObjects = new List<DataObject> { new Apparatus2 { Maker = masterObject, Exporter = masterObject } };

                InterfaceBusinessServer.NullifyMasterReferences(masterObject, referencingDataObjects, resultList);
            });
            Assert.IsType(typeof(PropertyCouldnotBeNullException), exception);
        }
    }
}