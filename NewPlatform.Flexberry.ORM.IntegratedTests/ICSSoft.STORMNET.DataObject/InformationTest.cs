namespace ICSSoft.STORMNET.Tests.TestClasses.DataObject
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.UserDataTypes;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Юнит-тесты для класса Informations.
    /// </summary>
    public class InformationTest
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly List<string> _log = new List<string>();

        /// <summary>
        /// Тест метода <see cref="GetView()"/>, позволяющего получить представление по его имени и классу объекта данных.
        /// </summary>
        [Fact]
        public void GetViewTest()
        {
            var threads = new List<Thread>();
            for (var i = 0; i < 10; i++)
            {
                var t = new Thread(RunMe);
                threads.Add(t);
                t.Start();
            }

            bool running;
            do
            {
                running = false;
                foreach (var thread in threads.Where(thread => thread.IsAlive))
                {
                    running = true;
                }
            }
            while (running);

            // 14750,2832 ms без кэша
            // 390,6825 ms для получения 2000 представлений с кэшем
            foreach (string s in _log)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// Специальный метод, применяющийся в <see cref="GetView()"/>.
        /// </summary>
        public void RunMe()
        {
            //// DateTime startTime = DateTime.Now;
            //// List<View> views = new List<View>();
            //// for (int i = 0; i < 1000; i++)
            //// {
            //// views.Add(Information.GetView("ПользовательE", typeof(Пользователь)));
            //// views.Add(Information.GetView("РольПользователяE", typeof(РольПользователя)));
            //// }

            //// DateTime endTime = DateTime.Now;
            //// log.Add(
            //// (endTime - startTime).TotalMilliseconds + " ms для получения " + views.Count + " представлений");
        }

        /// <summary>
        /// Проверка, что по лямбда-выражению класс Information может адекватно вернуть строковое представление имени поля.
        /// Например, есть InformationTestClass со свойством stringPropertyForInformationTestClass. По InformationTestClass.stringPropertyForInformationTestClass хотелось бы получить строку вида "InformationTestClass.stringPropertyForInformationTestClass".
        /// </summary>
        [Fact]
        public void TestExtractPropertyPath()
        {
            Assert.Equal("StringPropertyForInformationTestClass", Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));
            Assert.Equal("InformationTestClass.StringPropertyForInformationTestClass", Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(
                "InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass", Information.ExtractPropertyPath<InformationTestClass3>(c => c.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(
                "MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass",
                Information.ExtractPropertyPath<InformationTestClass4>(c => c.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(
                "InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass",
                Information.ExtractPropertyPath<ClassWithCaptions>(c => c.InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(
                "ExampleOfClassWithCaptions.InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass",
                Information.ExtractPropertyPath<InformationTestClass6>(c => c.ExampleOfClassWithCaptions.InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
        }

        /// <summary>
        /// Проверка, что по лямбда-выражению класс Information может адекватно вернуть строковое представление неполного имени поля.
        /// Например, есть InformationTestClass со свойством stringPropertyForInformationTestClass. По InformationTestClass.stringPropertyForInformationTestClass хотелось бы получить строку вида "stringPropertyForInformationTestClass".
        /// </summary>
        [Fact]
        public void TestExtractPropertyName()
        {
            Assert.Equal("StringPropertyForInformationTestClass", Information.ExtractPropertyName<InformationTestClass>(c => c.StringPropertyForInformationTestClass));
            Assert.Equal("StringPropertyForInformationTestClass", Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal("StringPropertyForInformationTestClass", Information.ExtractPropertyName<InformationTestClass3>(c => c.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(
                "StringPropertyForInformationTestClass", Information.ExtractPropertyName<InformationTestClass4>(c => c.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(
                "StringPropertyForInformationTestClass", Information.ExtractPropertyName<ClassWithCaptions>(c => c.InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(
                "StringPropertyForInformationTestClass",
                Information.ExtractPropertyName<InformationTestClass6>(c => c.ExampleOfClassWithCaptions.InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));
        }

        /// <summary>
        /// Тест метода <see cref="AllView()"/>, получающего список имён общих представлений для для указанного класса объекта данных.
        /// </summary>
        [Fact]
        public void AllViewTest()
        {
            // Ожидаемый результат: все представления, соответствующие этому классу.
            string[] expected = { "Test_MasterClassE", "Test_MasterClassL", "MasterClassE", "MasterClassL" };

            // Входной параметр: Тип данных: MasterClass.
            var actual = Information.AllViews(typeof(MasterClass));
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyCaption()"/>, возвращающего загаловок свойства.
        /// </summary>
        [Fact]
        public void GetPropertyCaptionTest()
        {
            // Ожидаемый результат: "stringPropertyForInformationTestClass".
            const string Expected = "StringPropertyForInformationTestClass";

            // Входные параметры: Тип даных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual = Information.GetPropertyCaption(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.Equal(Expected, actual);
            var actual13 = Information.GetPropertyCaption(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.Equal(Expected, actual13);

            // Ожидаемый результат: "DetailClass".
            const string Expected1 = "DetailClass";

            // Входные параметры: Тип даных: DetailClass, имя свойства: "MasterClass.DetailClass".
            var actual1 = Information.GetPropertyCaption(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));
            Assert.Equal(Expected1, actual1);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyStrLen()"/>, получающего для указанного .Net-свойства атрибут StrLen.
        /// </summary>
        [Fact]
        public void GetPropertyStrLenTest()
        {
            // Ожидаемый результат: целое число равное 255.
            const int Expected = 255;

            // Входные параметры: Тип данных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual = Information.GetPropertyStrLen(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));
            var actual0 = Information.GetPropertyStrLen(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.Equal(Expected, actual);

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.Equal(Expected, actual0);

            // Ожидаемый результат: целое число равное -1.
            const int NotExpected = -1;

            // Входные параметры: Тип даных: DetailClass, имя свойства: "MasterClass.DetailClass".
            var actual1 = Information.GetPropertyStrLen(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));
            Assert.Equal(NotExpected, actual1);
        }

        /// <summary>
        /// Тест метода <see cref="CheckNotNullAttributes()"/>, проверяющего, нет ли непустых значений в NotNull .Net-свойствах.
        /// </summary>
        [Fact]
        public void CheckNotNullAttributesTest()
        {
            // Входной параметр: объект данных типа InformationTestClass.
            var obj = new InformationTestClass();

            // Ожидаемый результат: массив строк равный null.
            var actual = Information.CheckNotNullAttributes(obj);
            Assert.Equal(null, actual);

            // Входной параметр: объект данных типа MasterClass.
            var obj1 = new MasterClass();

            // Ожидаемый результат: массив строк равный null.
            var actual1 = Information.CheckNotNullAttributes(obj1);
            Assert.Equal(actual1, null);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyDefineClassType()"/>, возвращающего тип в котором определено свойство.
        /// </summary>
        [Fact]
        public void GetPropertyDefineClassTypeTest()
        {
            // Ожидаемый результат: Тип данных: InformationTestClass.
            var expected = typeof(InformationTestClass);

            // Входный параметры: Тип данных: InformationTestClass, имя свойства "stringPropertyForInformationTestClass".
            var actual = Information.GetPropertyDefineClassType(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));
            var actual0 = Information.GetPropertyDefineClassType(
              typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.Equal(expected, actual);

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.Equal(expected, actual0);

            // Ожидаемый результат: Тип данных: MasterClass.
            var expected1 = typeof(MasterClass);

            // Входный параметры: Тип данных: DetailClass, имя свойства "MasterClass.DetailClass".
            var actual1 = Information.GetPropertyDefineClassType(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));
            Assert.Equal(expected1, actual1);

            // Входный параметры: Тип данных: InformationTestClass, имя свойства "__PrimaryKey".
            Information.GetPropertyDefineClassType(
             typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.__PrimaryKey));
        }

        /// <summary>
        /// Тест метода <see cref="GetCompatibleTypesForTypeConvertion()"/>, определяющего куда можно мконвертировать тип.
        /// </summary>
        [Fact]
        public void GetCompatibleTypesForTypeConvertionTest()
        {
            // Ожидаемый результат: множество, состоящее из типа InformationTestClass.
            Type[] expected = { typeof(InformationTestClass) };

            // Входные параметры:Имп данных: InformationTestClass.
            var actual = Information.GetCompatibleTypesForTypeConvertion(typeof(InformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.True(EquivalenceMethods.EqualTypeArrays(expected, actual));
            var actual0 = Information.GetCompatibleTypesForTypeConvertion(typeof(InformationTestClass));

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.True(EquivalenceMethods.EqualTypeArrays(expected, actual0));
        }

        /// <summary>
        /// Тест метода <see cref="GetAssemblyStorageName()"/>, получающего имя хранения для сборки, заданное атрибутом .
        /// </summary>
        [Fact]
        public void GetAssemblyStorageNameTest()
        {
            // Ожидаемый результат: пустая строка.
            var expected = string.Empty;

            // Входные параметры: Тип данных: InformationTestClass.
            var actual = Information.GetAssemblyStorageName(typeof(InformationTestClass));
            var actual0 = Information.GetAssemblyStorageName(typeof(InformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.Equal(expected, actual);

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.Equal(expected, actual0);
        }

        /// <summary>
        /// Тест метода <see cref="CheckPropertyExist()"/>, проверяющего наличие такого свойства в указанном типе.
        /// </summary>
        [Fact]
        public void CheckPropertyExistTest()
        {
            // Входные параметры: Тип данных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual = Information.CheckPropertyExist(
                typeof(InformationTestClass), Information.ExtractPropertyName<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Ожидаемый результат: true.
            Assert.True(actual);
        }

        /// <summary>
        /// Тест метода <see cref="GetNotStorablePropertyNames()"/>, возвращающего имена .Net-свойств для .Net-типа класса объекта данных, которые не хранятся .
        /// </summary>
        [Fact]
        public void GetNotStorablePropertyNamesTest()
        {
            // Ожидаемый результат: список свойств класса InformationTestClass, которые не хранятся.
            string[] expected = { "IsReadOnly", "DynamicProperties", "__PrototypeKey", "Prototyped" };

            // Входные параметры: Тип данных: InformationTestClass.
            var actual = Information.GetNotStorablePropertyNames(typeof(InformationTestClass));
            var actual0 = Information.GetNotStorablePropertyNames(typeof(InformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual0));
        }

        /// <summary>
        /// Тест метода <see cref="CanWriteProperty()"/>, определяющего можно ли писать в это свойство.
        /// </summary>
        [Fact]
        public void CanWritePropertyTest()
        {
            // Входные параметры: тип данных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual = Information.CanWriteProperty(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода. Ожидаемый результат: true.
            Assert.True(actual);

            // Входные параметры: тип данных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual0 = Information.CanWriteProperty(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием. Ожидаемый результат: true.
            Assert.True(actual0);

            // Входные параметры: тип данных: InformationTestClass2, имя свойства: "InformationTestClass.stringPropertyForInformationTestClass".
            var actual2 = Information.CanWriteProperty(
                typeof(InformationTestClass2), Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));

            // Ожидаемый результат: true.
            Assert.True(actual2);

            // Входные параметры: тип данных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual1 = Information.CanReadProperty(
               typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));

            // Ожидаемый результат: true.
            Assert.True(actual1);
        }

        /// <summary>
        /// Тест метода <see cref="CanReadProperty()"/>, определяющего можно ли читать из этого свойства.
        /// </summary>
        [Fact]
        public void CanReadPropertyTest()
        {
            // Входные параметры: тип данных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual = Information.CanReadProperty(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));
            var actual0 = Information.CanReadProperty(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));

            // Ожидаемый результат: true.
            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.True(actual);

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.True(actual0);

            // Входные параметры: тип данных: DetailClass, имя свойства: "MasterClass.DetailClass".
            var actual1 = Information.CanReadProperty(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));

            // Ожидаемый результат: true.
            Assert.True(actual1);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyNamesByType()"/>, возвращающего список свойств указанного шаблонного типа для .Net-класса объекта данных.
        /// </summary>
        [Fact]
        public void GetPropertyNamesByTypeTest()
        {
            // Ожидаемый результат: список содержащий элемент "stringPropertyForInformationTestClass".
            string[] expected = { "StringPropertyForInformationTestClass", "PublicStringProperty" };

            // Входные параметры: Тип даных: InformationTestClass, строковой тип данных.
            var actual = Information.GetPropertyNamesByType(typeof(InformationTestClass), typeof(String));

            // Вызываем второй раз чтобы достать значение из кеша
            var actual0 = Information.GetPropertyNamesByType(typeof(InformationTestClass), typeof(String));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual0));
        }

        /// <summary>
        /// Тест метода <see cref="GetClassCaptionProperty()"/>, возвращающего свойство - заголовок, установленное атрибутом <see cref="InstanceCaptionPropertyAttribute"/>.
        /// </summary>
        [Fact]
        public void GetClassCaptionPropertyTest()
        {
            // Ожидаемый результат: пустая строка.
            var expected = string.Empty;

            var actual1 = Information.GetClassCaptionProperty(typeof(ClassWithCaptions));
            Assert.Equal("Класс пятый для экземпляров", actual1);

            // Входные параметры: Тип данных: InformationTestClass.
            var actual = Information.GetClassCaptionProperty(typeof(InformationTestClass));

            // Вызываем второй раз чтобы достать значение из кеша
            var actual0 = Information.GetClassCaptionProperty(typeof(InformationTestClass));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.Equal(expected, actual);

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.Equal(expected, actual0);
        }

        /// <summary>
        /// Тест метода <see cref="IsEmptyPropertyValue()"/>, определяющего является ли значение пустым (null).
        /// </summary>
        [Fact]
        public void IsEmptyPropertyValueTest()
        {
            // Входной парметр: пустое значение.
            var actual1 = Information.IsEmptyPropertyValue(null);
            Assert.True(actual1);

            // Входной парметр: пустая строгка.
            var actual2 = Information.IsEmptyPropertyValue(string.Empty);
            Assert.True(actual2);

            // Входной парметр: пустое перечислимое значение.
            const SwappedEnum Se = new SwappedEnum();
            var actual3 = Information.IsEmptyPropertyValue(Se);
            Assert.False(actual3);

            // Пошлем в него пустой Enum
            var actual5 = Information.IsEmptyPropertyValue(CaseSensitiveEnum.CaseINSENSITIVEVAL);
            Assert.True(actual5);

            // Пошлем в него класс, реализующий интерфейс ISpecialEmptyValue
            var actual6 = Information.IsEmptyPropertyValue(new SpecialEmptyValueForTest());
            Assert.True(actual6);
        }

        /// <summary>
        /// Тест метода <see cref="Information.GetPrimaryKeyStorageName"/>, получающего имя хранения первичного ключа, установленного атрибутом <see cref="PrimaryKeyStorageAttribute"/>.
        /// </summary>
        [Fact]
        public void GetPrimaryKeyStorageNameTest()
        {
            string actual1 = Information.GetPrimaryKeyStorageName(typeof(StoredClass));
            Assert.Equal("primaryKey", actual1);
        }

        /// <summary>
        /// Тест метода <see cref="GetStorageTypeForType()"/>, возвращающего тип хранения для заданного типа.
        /// </summary>
        [Fact(Skip = "разобраться что это за тест такой")]
        public void GetStorageTypeForTypeTest()
        {
            // TODO: разобраться что это за тест такой.
            /*
            Type type = Information.GetStorageTypeForType(typeof(DetailArraySubClass), typeof(MSSQLDataService));
            Assert.Equal("DetailArraySubClass", type.Name);
             */
        }

        /// <summary>
        /// Тест метода <see cref="IsEmptyEnumValue()"/>, определяющего является ли значение перечислимого типа пустым (null).
        /// </summary>
        [Fact]
        public void IsEmptyEnumValueTest()
        {
            var actual1 = Information.IsEmptyEnumValue(null);
            Assert.True(actual1);

            var actual2 = Information.IsEmptyEnumValue(SwappedEnum.Val);
            Assert.False(actual2);

            var actual3 = Information.IsEmptyEnumValue(SwappedEnum.SwappedVal);
            Assert.True(actual3);

            var actual4 = Information.IsEmptyEnumValue(CaseSensitiveEnum.CaseINSENSITIVEVAL);
            Assert.True(actual4);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyDataFormat()"/>, получающего формат представления данных в свойстве.
        /// </summary>
        [Fact]
        public void GetPropertyDataFormatTest()
        {
            // Ожидаемый результат: пустая строка.
            var expected = string.Empty;

            // Входные параметры: Тип данных: InformationTestClass, имя свойства: "stringPropertyForInformationTestClass".
            var actual = Information.GetPropertyDataFormat(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass));
            Assert.Equal(expected, actual);

            // Ожидаемый результат: пустая строка.
            var expected1 = string.Empty;

            // Входные параметры: Тип данных: InformationTestClass2, имя свойства: "InformationTestClass.stringPropertyForInformationTestClass".
            var actual1 = Information.GetPropertyDataFormat(
                typeof(InformationTestClass2), Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.Equal(expected1, actual1);
        }

        /// <summary>
        /// Тест метода <see cref="GetAlteredPropertyNamesWithNotStored()"/>, сравнивающего два объекта данных
        /// и возвращающего список различающихся .Net-свойств. (NotStored-атрибуты не игнорируются и тоже проверяются вместе с остальными).
        /// </summary>
        [Fact]
        public void GetAlteredPropertyNamesWithNotStoredTest()
        {
            var obj1 = new InformationTestClass();
            var obj2 = new InformationTestClass();
            string[] expected = { "__PrimaryKey" };

            // Входные параметры: объект данных класса InformationTestClass, объект даных класса InformationTestClass2, без сравнения детейловых объектов.
            var actual = Information.GetAlteredPropertyNamesWithNotStored(obj1, obj2, false);

            // Ожидаемый результат: "__PrimaryKey".
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));

            // Входные параметры: объект данных класса InformationTestClass, объект даных класса InformationTestClass2, со сравнением детейловых объектов.
            var actual1 = Information.GetAlteredPropertyNamesWithNotStored(obj1, obj2, true);

            // Ожидаемый результат: "__PrimaryKey".
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual1));
            var expected0 = new string[0];

            // Входные параметры: null, null, без сравнения детейловых объектов.
            var actual2 = Information.GetAlteredPropertyNamesWithNotStored(null, null, true);

            // Ожидаемый результат: пустой массив строк.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected0, actual2));

            // Входные параметры: null, null, со сравнением детейловых объектов.
            var actual3 = Information.GetAlteredPropertyNamesWithNotStored(null, null, false);

            // Ожидаемый результат: пустой массив строк.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected0, actual3));

            string[] expected1 = typeof(InformationTestClass).GetProperties().Select(property => property.Name).ToArray();

            // Входные параметры: null, объект данных класса InformationTestClass, без сравнения детейловых объектов.
            var actual4 = Information.GetAlteredPropertyNamesWithNotStored(null, obj1, false);

            // Ожидаемый результат: массив строк, содержащий имена свойств объекта даных класса InformationTestClass.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected1, actual4));

            // Входные параметры: null, объект данных класса InformationTestClass, со сравнением детейловых объектов.
            var actual5 = Information.GetAlteredPropertyNamesWithNotStored(null, obj1, true);

            // Ожидаемый результат: массив строк, содержащий имена свойств объекта даных класса InformationTestClass.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected1, actual5));

            // Входные параметры: объект данных класса InformationTestClass, null, со сравнением детейловых объектов.
            var actual6 = Information.GetAlteredPropertyNamesWithNotStored(obj1, null, true);

            // Ожидаемый результат: массив строк, содержащий имена свойств объекта даных класса InformationTestClass.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected1, actual6));

            // Входные параметры: объект данных класса InformationTestClass, null, без сравнения детейловых объектов.
            var actual7 = Information.GetAlteredPropertyNamesWithNotStored(obj1, null, false);

            // Ожидаемый результат: массив строк, содержащий имена свойств объекта даных класса InformationTestClass.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected1, actual7));

            // Входные параметры: новый объект данных класса MasterClass, новый объект данных класса MasterClass, со сравнением детейловых объектов.
            var actual8 = Information.GetAlteredPropertyNamesWithNotStored(new MasterClass(), new MasterClass(), true);

            // Ожидаемый результат: "__PrimaryKey".
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual8));

            // Входные параметры: новый объект данных класса MasterClass, новый объект данных класса MasterClass, без сравнения детейловых объектов.
            var actual9 = Information.GetAlteredPropertyNamesWithNotStored(new MasterClass(), new MasterClass(), false);

            // Ожидаемый результат: "__PrimaryKey".
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual9));
        }

        /// <summary>
        /// Тест метода <see cref="GetClassImageProperty()"/>, возвращающего свойство-картинку, установленное атрибутом <see cref="ClassImagePropertyAttribute"/>.
        /// </summary>
        [Fact]
        public void GetClassImagePropertyTest()
        {
            var expected = string.Empty;

            // Входной параметр: объект данных класса InformationTestClass.
            var actual = Information.GetClassImageProperty(typeof(InformationTestClass));

            // Ожидаемый результат: пустое значение.
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Тест метода <see cref="RetrieveLinkerTimestamp()"/>, получающего дату компиляции текущей сборки.
        /// </summary>
        [Fact]
        public void RetrieveLinkerTimestampTest()
        {
            // Выходные параметры: дата и время.
            var actual = Information.RetrieveLinkerTimestamp();
            System.Diagnostics.Debug.WriteLine(actual);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyStorageType()"/>, возвращающего тип хранения для заданного свойства.
        /// </summary>
        [Fact]
        public void GetPropertyStorageTypeTest()
        {
            // Ожидаемый результат: строковвой тип данных.
            var expected = typeof(String);

            // Входные даные: тип данных: InformationTestClass, имя свойства: stringPropertyForInformationTestClass, тип свойства - строка.
            var actual = Information.GetPropertyStorageType(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass), typeof(String));
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Тест метода <see cref="AllViews()"/>, получающий список имён общих представлений для указанных классов.
        /// </summary>
        [Fact]
        public void AllViewsTest()
        {
            var expected = new string[0];
            var typearr = new Type[2];
            typearr[0] = typeof(MasterClass);
            typearr[1] = typeof(DetailClass);

            // Входной параметр: массив типов {MasterClass, DetailClass}.
            var actual = Information.AllViews(typearr);

            // Ожидаемый результат: пустой массив строк.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));
            string[] expected1 = { "MasterClassL", "MasterClassE", "Test_MasterClassL", "Test_MasterClassE" };
            var typearr1 = new Type[2];
            typearr1[0] = typeof(MasterClass);
            typearr1[1] = typeof(MasterClass);

            // Входной параметр: массив типов {MasterClass, MasterClass}.
            var actual1 = Information.AllViews(typearr1);

            // Ожидаемый результат: массив строк, содержащий имена всех свойств класса MasterClass.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected1, actual1));

            // Входной параметр: пустой массив типов.
            var actual2 = Information.AllViews();

            // Ожидаемый результат: null
            Assert.True(EquivalenceMethods.EqualStringArrays(new string[0], actual2));
        }

        /// <summary>
        /// Тест выпадения исключения ArgumentNullException в методе  GetView()
        /// при попытке получения представления по пустому имени.
        /// </summary>
        [Fact]
        public void GetViewNullExTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetView(null, typeof(MasterClass));
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения ArgumentNullException в методе  GetView()
        /// при попыткеполучения представления для пустого типа данных.
        /// </summary>
        [Fact]
        public void GetViewNullExTest1()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetView("ПользоывательE", null);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="GetKeyGenerator()"/>,  получающий тип генератора ключей.
        /// </summary>
        [Fact]
        public void GetKeyGeneratorTypeTest()
        {
            // Ожидаемый результат: тип данных - генератор ключей.
            var expected = typeof(GUIDGenerator);

            // Входной параметр: тип данных -  InformationTestClass.
            var actual = Information.GetKeyGeneratorType(typeof(InformationTestClass));
            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Тест метода <see cref="GetTypeStorageName()"/>, получающего имя хранилища для типа.
        /// </summary>
        [Fact]
        public void GetTypeStorageNameTest()
        {
            // Ожидаемый результат - имя хранилища для типа: "typeId".
            const string Expected = "typeId";

            // Входной параметр: Тип: InformationTestClass.
            var actual = Information.GetTypeStorageName(typeof(InformationTestClass));
            Assert.Equal(Expected, actual);
        }

        /// <summary>
        /// Тест метода <see cref="CheckViewForClasses()"/>,  проверяющего, доступно ли указанное по имени представление во всех перечисленных классах.
        /// </summary>
        [Fact]
        public void CheckViewForClassesTest()
        {
            const bool Expected = false;
            var myType1 = typeof(MasterClass);
            var myType2 = typeof(DetailClass);
            var typearr = new Type[2];
            typearr[0] = myType1;
            typearr[1] = myType2;

            // Входные параметры: Представление: MasterClassЕ, массив типов: {MasterClass, DetailClass}.
            var actual = Information.CheckViewForClasses("MasterClassE", typearr);

            // Ожидаемый пезультат: false.
            Assert.Equal(Expected, actual);
            var typearr1 = new Type[0];

            // Входные параметры: Представление: MasterClassЕ, пустой массив.
            var actual1 = Information.CheckViewForClasses("MasterClassE", typearr1);

            // Ожидаемый пезультат: False.
            Assert.Equal(Expected, actual1);
            var typearr2 = new Type[3];
            typearr2[0] = myType1;
            typearr2[1] = myType2;
            typearr2[2] = typeof(InformationTestClass);

            // Входные параметры: Представление: MasterClassЕ, массив типов:{MasterClassс,Detaillass,InformationTestClass}.
            var actual2 = Information.CheckViewForClasses("MasterClassE", typearr2);

            // Ожидаемый пезультат: false.
            Assert.Equal(Expected, actual2);
            var typearr3 = new Type[3];
            typearr3[0] = myType1;
            typearr3[1] = myType1;
            typearr3[2] = myType2;

            // Входные параметры: Представление: MasterClassЕ, массив типов: {MasterClass, MasterClass, DetailClass}.
            var actual3 = Information.CheckViewForClasses("MasterClassE", typearr3);

            // Ожидаемый пезультат: False.
            Assert.Equal(Expected, actual3);
        }

        /// <summary>
        /// Тест метода <see cref="GetCompatibleTypesForProperty()"/>, возвращающего типы, совместимые с данным свойством(по TypeUsage).
        /// </summary>
        [Fact]
        public void GetCompatibleTypesForPropertyTest()
        {
            // Ожидаемое значение: множетво, состоящее из строкового типа.(для обоих случаев)
            Type[] expected = { typeof(String) };

            // Входные параметры: тип класа: MasterClass, имя свойства: "StringMasterProperty".
            var actual = Information.GetCompatibleTypesForProperty(
                typeof(MasterClass), Information.ExtractPropertyName<MasterClass>(c => c.StringMasterProperty));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.True(EquivalenceMethods.EqualTypeArrays(expected, actual));

            // Входные параметры: тип класа: MasterClass, имя свойства: "StringMasterProperty".
            var actual0 = Information.GetCompatibleTypesForProperty(
                typeof(MasterClass), Information.ExtractPropertyName<MasterClass>(c => c.StringMasterProperty));

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.True(EquivalenceMethods.EqualTypeArrays(expected, actual0));

            // Входные параметры: тип класа: InformationTestClass2, имя свойства: "InformationTestClass.stringPropertyForInformationTestClass".
            var actual1 = Information.GetCompatibleTypesForProperty(
                typeof(InformationTestClass2), Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            Assert.True(EquivalenceMethods.EqualTypeArrays(expected, actual1));
        }

        /// <summary>
        /// Тест выпадения исключения CantFindPropertyException в классе GetPropertyCaption
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void GetPropertyCaptionExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetPropertyCaption(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(CantFindPropertyException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyStorageName()"/>, получающего имя хранения .Net-свойства, установленное атрибутом <see cref="PropertyStorageAttribute"/>.
        /// </summary>
        [Fact]
        public void GetPropertyStorageNameTest()
        {
            // Ожидаемое значение: "primaryKey".
            const string Expected = "primaryKey";

            // Входные параметры: Тип класса: MasterClass, имя свойства: "__PrimaryKey", индекс: 1.
            var actual = Information.GetPropertyStorageName(typeof(MasterClass), "__PrimaryKey", 1);

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода.
            Assert.Equal(Expected, actual);

            var actual0 = Information.GetPropertyStorageName(typeof(MasterClass), "__PrimaryKey", 1);

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием.
            Assert.Equal(Expected, actual0);
        }

        /// <summary>
        /// Тест выпадения исключения CantFindPropertyException в методе GetPropertyStorageNamе()
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void GetPropertyStorageNameExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetPropertyStorageName(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass), 1);
            });
            Assert.IsType(typeof(CantFindPropertyException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyNotNull()"/>, проверчющего, установлен ли для указанного .Net-свойства атрибут <see cref="NotNullAttribute"/>.
        /// </summary>
        [Fact]
        public void GetPropertyNotNullTest()
        {
            // Входные параметры: тип данных: InformationTestClass2, свойство: InformationTestClass.stringPropertyForInformationTestClass.
            var actual = Information.GetPropertyNotNull(
                typeof(InformationTestClass2), Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));

            // Ожидаемый результат: false.
            Assert.False(actual);
        }

        /// <summary>
        /// Тест выпадения исключения CantFindPropertyException в методе GetPropertyNotNull()
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void GetPropertyNotNullExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetPropertyNotNull(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(CantFindPropertyException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения CantFindPropertyException в методе GetPropertyStrLen()
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void GetPropertyStrLenExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetPropertyStrLen(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(CantFindPropertyException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения CantFindPropertyException в методе GetPropertyDefineClassType()
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void GetPropertyDefineClassTypeExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetPropertyDefineClassType(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(CantFindPropertyException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения CantFindPropertyException в методе GetPropertyType()
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void GetPropertyTypeExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetPropertyType(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(CantFindPropertyException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения NoSuchPropertyException в методе IsStoredProperty()
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void IsStoredPropertyExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.IsStoredProperty(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(NoSuchPropertyException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="IsStoredProperty()"/>, проверяющего, является ли поле хранимым.
        /// </summary>
        [Fact]
        public void IsStoredPropertyTest()
        {
            // Проверим хранимое поле
            var actual1 = Information.IsStoredProperty(typeof(StoredClass), "StoredProperty");

            // Второй вызов для доставания значения из кеша.
            var actual2 = Information.IsStoredProperty(typeof(StoredClass), "StoredProperty");

            Assert.True(actual1);
            Assert.True(actual2);

            // Проверим нехранимое поле
            var actual3 = Information.IsStoredProperty(typeof(StoredClass), "NotStoredProperty");
            Assert.False(actual3);
        }

        /// <summary>
        /// Тест выпадения исключения CantFindPropertyExceptionв методе GetPropertyDataFormat()
        /// при попытке найти значение класса InformationTestClass в классе InformationTestClass2.
        /// </summary>
        [Fact]
        public void GetPropertyDataFormatExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetPropertyDataFormat(
                    typeof(InformationTestClass2), Information.ExtractPropertyName<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(CantFindPropertyException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="GetAlteredProperyNames()"/>, cравнивающего два объекта данных и возвращающего список различающихся .Net-свойств.
        ///  (Объект или свойство с атрибутом NotStored проверяться не будет).
        /// </summary>
        [Fact]
        public void GetAlteredProperyNamesTest()
        {
            string[] expected = { };

            // Входные параметры: null, null, сравнения по детейлам нет.
            var actual = Information.GetAlteredPropertyNames(null, null, false);

            // Ожидаемый результат: пустое множество.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual));

            // Входные параметры: null, null, сравнение по детейлам есть.
            var actual1 = Information.GetAlteredPropertyNames(null, null, true);

            // Ожидаемый результат: пустое множество.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected, actual1));
            var obj = new MasterClass();

            string[] expected2 = obj.GetInitializedProperties(true).ToArray();

            // Входные параметры: null, объект данных типа MasterClass, сравнение по детейлам есть.
            var actual2 = Information.GetAlteredPropertyNames(null, new MasterClass(), true);

            // Ожидаемый результат: множество, состоящее из имен полей класса MasterClass "StringMasterProperty", "DetailClass", "__PrimaryKey".
            Assert.True(EquivalenceMethods.EqualStringArrays(expected2, actual2));

            // Входные параметры: null, объект данных типа MasterClass, сравнения по детейлам нет.
            var actual3 = Information.GetAlteredPropertyNames(null, new MasterClass(), false);

            // Ожидаемый результат: множество, состоящее из имен полей класса MasterClass "StringMasterProperty", "DetailClass", "__PrimaryKey".
            Assert.True(EquivalenceMethods.EqualStringArrays(expected2, actual3));
            string[] expected4 = { "__PrimaryKey" };

            // Входные параметры: объект данных типа MasterClass, объект данных типа MasterClass, сравнение по детейлам есть.
            var actual4 = Information.GetAlteredPropertyNames(new MasterClass(), new MasterClass(), true);

            // Ожидаемый результат: множество, состоящее из имени поля "__PrimaryKey" класса MasterClass.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected4, actual4));

            // Входные параметры: объект данных типа MasterClass, объект данных типа MasterClass, сравнения по детейлам нет.
            var actual5 = Information.GetAlteredPropertyNames(new MasterClass(), new MasterClass(), false);

            // Ожидаемый результат: множество, состоящее из имени поля "__PrimaryKey" класса MasterClass.
            Assert.True(EquivalenceMethods.EqualStringArrays(expected4, actual5));
        }

        /// <summary>
        /// Тест выпадения системного исключения в методе CheckPropertyExist()
        /// при пустом типе объекта даных, существование свойства которого проверяется данным методом.
        /// </summary>
        [Fact]
        public void CheckPropertyExistExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.CheckPropertyExist(null, Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));
            });
            Assert.IsType(typeof(Exception), exception);
        }

        /// <summary>
        /// Тест метода <see cref="CheckUsingType()"/>, выполняющего проверку на совместимость объекта данных в  методе, или свойстве, откуда вызвано.
        /// </summary>
        [Fact]
        public void CheckUsingTypeTest()
        {
            // Входные параметры: объект класса InformationTestClass, объект класса DetailClass, пустое значение.
            var obj1 = new InformationTestClass();
            var obj2 = new DetailClass();

            // Ожидаемый результат: отсутствиключений при срабатывании метода.
            Information.CheckUsingType(obj1);
            Information.CheckUsingType(null);
            Information.CheckUsingType(obj2);
        }

        /// <summary>
        /// Тест выпадения исключения PrimaryKeyTypeException в методе TranslateValueToPrimaryKeyType()
        /// при значении входных параметров : тип объекта данных=typeof(InformationTestClass)
        /// и преобразуемое значение - число.
        /// </summary>
        [Fact]
        public void TranslateValueToPrimaryKeyTypeExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                const int NotPrimaryKey = 0;
                Information.TranslateValueToPrimaryKeyType(typeof(InformationTestClass), NotPrimaryKey);
            });
            Assert.IsType(typeof(PrimaryKeyTypeException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения TargetInvocationException в методе TranslateValueToPrimaryKeyTypeFormat()
        /// при значени входных параметров: тип объекта данных=typeof(InformationTestClass)
        /// и преобразуемое значение - строка, формат котрой не соответствует формату первичного ключа.
        /// </summary>
        [Fact]
        public void TranslateValueToPrimaryKeyTypeFormatExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.TranslateValueToPrimaryKeyType(typeof(InformationTestClass), "Эта строка не является первичным ключём");
            });
            Assert.IsType(typeof(TargetInvocationException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="GetMastersForDataObjectByView()"/>, возвращающего все мастеровые объекты данных для указанного объекта данных.
        /// </summary>
        [Fact]
        public void GetMastersForDataObjectByViewTest()
        {
            // Ожидаемое значение: пустой список объектов.
            var expected = new List<object>();

            // Входные параметры: объект данных класса детейла: new DetailClass(), представление: DetailClassE.
            var actual = Information.GetMastersForDataObjectByView(new DetailClass(), "DetailClassE").ToList<object>();
            Assert.True(EquivalenceMethods.ListEquals(expected, actual));

            var actual1 = Information.GetMastersForDataObjectByView(new DetailClass(), "DetailClassНетТакогоПредставления").ToList<object>();
            Assert.True(actual1.Count == 0);

            var obj = new MasterClass
            {
                InformationTestClass = new InformationTestClass { StringPropertyForInformationTestClass = "Ололо", PublicStringProperty = "Атата" },
                InformationTestClass2 = new InformationTestClass2 { StringPropertyForInformationTestClass2 = "Ороро", InformationTestClass = new InformationTestClass { PublicStringProperty = "TTT" } }
            };

            var actual2 = Information.GetMastersForDataObjectByView(obj, "MasterClassL").ToList<object>();
            Assert.True(actual2.Count == 3);
        }

        /// <summary>
        /// Тест для метода <see cref="ExtractPropertyInfo()"/>, извлекающего свойство по типу.
        /// Ловим исключение, послав null заместо лямбды.
        /// </summary>
        [Fact]
        public void ExtractPropertyInfoTest0()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.ExtractPropertyInfo<InformationTestClass>(null);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Тест для метода <see cref="ExtractPropertyInfo()"/>, извлекающего свойство по типу.
        /// </summary>
        [Fact]
        public void ExtractPropertyInfoTest1()
        {
            var actual = Information.ExtractPropertyInfo<InformationTestClass>(c => c.StringPropertyForInformationTestClass);
            Assert.Equal(actual.Name, "StringPropertyForInformationTestClass");
            Assert.Equal(actual.PropertyType.Name, "String");
        }

        /// <summary>
        /// Тест для метода <see cref="ExtractPropertyName()"/>, извлекающего свойство по наименованию.
        /// Ловим исключение, послав не Свойство (не Property), а просто поле.
        /// </summary>
        [Fact(Skip = "разобраться нужно ли это проверять")]
        public void ExtractPropertyInfoTest2()
        {
            var exception = Xunit.Record.Exception(() =>
            {
            });
            Assert.IsType(typeof(ArgumentException), exception);

            // TODO: разобраться нужно ли это проверять.
            // Information.ExtractPropertyInfo<InformationTestClass2>(c => c.NotProperty);
        }

        /// <summary>
        /// Тест для метода <see cref="ExtractPropertyName()"/>, извлекающего свойство по наименованию.
        /// Ловим исключение, послав статическое свойство.
        /// </summary>
        [Fact(Skip = "разобраться нужно ли это проверять")]
        public void ExtractPropertyNameTest0()
        {
            var exception = Xunit.Record.Exception(() =>
            {
            });
            Assert.IsType(typeof(ArgumentException), exception);

            // TODO: разобраться нужно ли это проверять.
            // Information.ExtractPropertyName<InformationTestClass2>(c => InformationTestClass2.StaticProperty);
        }

        /// <summary>
        /// Тест для метода <see cref="ExtractPropertyName()"/>, извлекающего свойство по наименованию.
        /// Ловим исключение, послав null заместо лямбды.
        /// </summary>
        [Fact]
        public void ExtractPropertyNameTest1()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.ExtractPropertyName((Expression<Func<InformationTestClass2, object>>)null);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="ExtractPropertyName"/>, извлекающего свойства внутри текущего класса. 
        /// Входные параметры: Тип класса: InformationTestClass, лямбда выражение: () =&gt; obj.stringPropertyForInformationTestClass.
        /// Ожидаемое значение: "stringPropertyForInformationTestClass".
        /// </summary>
        [Fact]
        public void ExtractPropertyNameTest2()
        {
            var obj = new InformationTestClass();
            const string Expected = "StringPropertyForInformationTestClass";
            var actual = Information.ExtractPropertyName(() => obj.StringPropertyForInformationTestClass);
            Assert.Equal(Expected, actual);

            var value = Information.ExtractPropertyPath(() => obj);
            Assert.Equal(value, "obj");
        }

        /// <summary>
        /// Тест на выпадения исключения ArgumentException в методе ExtractPropertyName() при некоектном значении лямбда-выражения.
        /// </summary>
        [Fact]
        public void ExtractPropertyNameTest3()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.ExtractPropertyPath<InformationTestClass>(() => null);
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropValueByName()"/>, получающего значение свойства объекта данных по имени этого свойства.
        /// </summary>
        [Fact]
        public void GetPropValueByNameTest()
        {
            // Входные параметры: объект даных: null, имя свойства: "StringMasterProperty".
            var actual = Information.GetPropValueByName(
                null, Information.ExtractPropertyName<MasterClass>(c => c.StringMasterProperty));

            // Первый Assert вызывается для того, чтобы покрыть тестом основной функционал метода. Ожидаемое значение: null.
            Assert.Equal(null, actual);

            // Входные параметры: объект даных: null, имя свойства: "MasterClass.StringMasterProperty".
            var actual1 = Information.GetPropValueByName(
                null, Information.ExtractPropertyPath<MasterClass>(c => c.StringMasterProperty));

            // Второй раз Assert вызывается для того, чтобы покрыть ту часть метода, которая связана с кэшированием. Ожидаемое значение: null.
            Assert.Equal(null, actual1);
            var obj = new ClassWithCaptions();

            // Входные параметры: объект даных класса ClassWithCaptions, имя свойства: "InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.stringPropertyForInformationTestClass".
            var actual2 = Information.GetPropValueByName(
                obj, Information.ExtractPropertyPath<ClassWithCaptions>(c => c.InformationTestClass4.MasterOfInformationTestClass3.InformationTestClass2.InformationTestClass.StringPropertyForInformationTestClass));

            // Ожидаемое значение: null.
            Assert.Equal(null, actual2);
        }

        /// <summary>
        /// Тест метода <see cref="SetPropValueByName(DataObject obj, string propName, object PropValue)"/>, устанавливающего значение строкового свойства с отсечением пробелов.
        /// </summary>
        [Fact]
        public void SetPropValueByNameTrimStringTest()
        {
            // Arrange.
            var obj = new InformationTestClass();

            // Act.
            Information.SetPropValueByName(
                obj, Information.ExtractPropertyPath<InformationTestClass>(c => c.StringPropertyForInformationTestClass), " Test Value ");

            // Assert.
            // Строка-значение должна быть без оконечных пробелов.
            Assert.Equal("Test Value", obj.StringPropertyForInformationTestClass);
         }

        /// <summary>
        /// Тест выпадения исключения в методе SetPropValueByName(DataObject obj, string propName, string PropValue)
        /// при несоответствии типов записываемого значения и свойства объекта, в который производится запись.
        /// </summary>
        [Fact]
        public void SetPropValueByNameExceptionTest1()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var val = new object();
                Information.SetPropValueByName(
                    new InformationTestClass(), Information.ExtractPropertyName<InformationTestClass>(c => c.IntPropertyForInformationTestClass), val);
            });
            Assert.IsType(typeof(Exception), exception);
        }

        /// <summary>
        /// Тест выпадения исключения в методе SetPropValueByName(DataObject obj, string propName, string PropValue).
        /// </summary>
        [Fact]
        public void SetPropValueByNameExceptionTest2()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var obj = new MasterClass();
                Information.SetPropValueByName(
                    obj, Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.StringMasterProperty), "property_value");
            });
            Assert.IsType(typeof(Exception), exception);
        }

        /// <summary>
        /// Тест выпадения системного исключения в методе SetPropValueByName(DataObject obj, string propName, string PropValue)
        /// при значении параметра DataObject равным null.
        /// </summary>
        [Fact]
        public void SetPropValueByNameExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.SetPropValueByName(
                    null, Information.ExtractPropertyName<MasterClass>(c => c.StringMasterProperty), "StringMasterProperty");
            });
            Assert.IsType(typeof(Exception), exception);
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyType()"/>, получающего .Net-тип свойства класса объекта данных по имени этого свойства.
        /// </summary>
        [Fact]
        public void GetPropertyTypeTest()
        {
            var expected = typeof(KeyGuid);

            // Входные параметры: тип данных: InformationTestClass, имя свойства: __PrimaryKey.
            var actual = Information.GetPropertyType(typeof(InformationTestClass), "__PrimaryKey");

            // Ожидаемый результат: Тип данных: ICSSoft.STORMNET.KeyGen.KeyGuid.
            Assert.Equal(expected, actual);
            var expected1 = typeof(DetailArrayOfDetailClass);

            // Входные параметры: тип данных: DetailClass, имя свойства: MasterClass.DetailClass.
            var actual1 = Information.GetPropertyType(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));

            // Ожидаемый результат: Тип данных: DetailArrayOfDetailClass.
            Assert.Equal(expected1, actual1);
        }

        /// <summary>
        /// Тест выпадения исключения NullReferenceException в методе GetStorageStructForView()
        /// в том случае, если некоторые входные параметры равны null.
        /// </summary>
        [Fact]
        public void GetStorageStructForViewNullReferenceExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetStorageStructForView(
                    null, typeof(MasterClass), StorageTypeEnum.HierarchicalStorage, null, typeof(string));
            });
            Assert.IsType(typeof(NullReferenceException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения ClassIsNotSubclassOfOtherException в методе GetStorageStructForView()
        /// в том случае, если класс объекта данных, передаваемый, как параметр type не является подклассом класса, которому принадлежит представление.
        /// </summary>
        [Fact]
        public void GetStorageStructForViewClassIsNotSubclassOfOtherExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetStorageStructForView(
                    view: Information.GetView("MasterClassE", typeof(MasterClass)),
                    type: typeof(DetailClass),
                    storageType: STORMNET.Business.StorageTypeEnum.HierarchicalStorage,
                    getPropertiesInExpression: null,
                    DataServiceType: typeof(MasterClass));
            });
            Assert.IsType(typeof(ClassIsNotSubclassOfOtherException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения DifferentDataObjectTypesException в методе GetAlteredProperyNames()
        /// при различии типов сравниваемых объектов данных.
        /// </summary>
        [Fact]
        public void GetAlteredProperyNamesDifferentDataObjectTypesExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetAlteredPropertyNames(new MasterClass(), new DetailClass(), false);
            });
            Assert.IsType(typeof(DifferentDataObjectTypesException), exception);
        }

        /// <summary>
        /// Тест выпадения исключения DifferentDataObjectTypesException в методе GetAlteredPropertyNamesWithNotStored()
        /// при различии типов сравниваемых объектов данных.
        /// </summary>
        [Fact]
        public void GetAlteredPropertyNamesWithNotStoredDifferentDataObjectTypesExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetAlteredPropertyNamesWithNotStored(new MasterClass(), new DetailClass(), false);
            });
            Assert.IsType(typeof(DifferentDataObjectTypesException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="ContainsAlteredProps()"/>, сравнивающего два объекта данных и возвращающего true - если объекты различаются.
        /// </summary>
        [Fact]
        public void ContainsAlteredPropsTest()
        {
            // Входные параметры: тип данных InformationTestClass, тип даных: InformationTestClass, сравнение детейловых элементов есть.
            var actual = Information.ContainsAlteredProps(new InformationTestClass(), new InformationTestClass(), true);

            // Ожидаемый результат:true.
            Assert.True(actual);

            // Входные параметры: тип данных InformationTestClass, тип даных: InformationTestClass, сравнение детейловых элементов нет.
            var actual1 = Information.ContainsAlteredProps(new InformationTestClass(), new InformationTestClass(), false);

            // Ожидаемый результат:true.
            Assert.True(actual1);

            // Входные параметры: null, тип даных: InformationTestClass, сравнение детейловых элементов есть.
            var actual2 = Information.ContainsAlteredProps(null, new InformationTestClass(), true);

            // Ожидаемый результат:true.
            Assert.True(actual2);

            // Входные параметры: null, тип даных: InformationTestClass, сравнение детейловых элементов нет.
            var actual3 = Information.ContainsAlteredProps(null, new InformationTestClass(), false);

            // Ожидаемый результат:true.
            Assert.True(actual3);

            // Входные параметры: тип данных InformationTestClass, null, сравнение детейловых элементов есть.
            var actual4 = Information.ContainsAlteredProps(new InformationTestClass(), null, true);

            // Ожидаемый результат:true.
            Assert.True(actual4);

            // Входные параметры: тип данных InformationTestClass, null, сравнение детейловых элементов нет.
            var actual5 = Information.ContainsAlteredProps(new InformationTestClass(), null, false);

            // Ожидаемый результат:true.
            Assert.True(actual5);

            // Входные параметры: null, null, сравнение детейловых элементов есть.
            var actual6 = Information.ContainsAlteredProps(null, null, true);

            // Ожидаемый результат:false.
            Assert.False(actual6);

            // Входные параметры: null, null, сравнение детейловых элементов нет.
            var actual7 = Information.ContainsAlteredProps(null, null, false);

            // Ожидаемый результат:false.
            Assert.False(actual7);
        }

        /// <summary>
        /// Тест выпадения исключения DifferentDataObjectTypesException в методе ContainsAlteredProps()
        /// при различии типов сравниваемых объектов данных.
        /// </summary>
        [Fact]
        public void ContainsAlteredPropsDifferentDataObjectTypesExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.ContainsAlteredProps(new MasterClass(), new DetailClass(), false);
            });
            Assert.IsType(typeof(DifferentDataObjectTypesException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="TrimmedStringStorage()"/>, определяющего обрезать ли строки для данного свойства.
        /// </summary>
        [Fact]
        public void TrimmedStringStorageTest()
        {
            // Входные параметры: тип даных:DetailClass,свойство: MasterClass.StringMasterProperty.
            var actual = Information.TrimmedStringStorage(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.StringMasterProperty));

            // Ожидаемый результат: true.
            Assert.True(actual);
        }

        /// <summary>
        /// Тест метода <see cref="GetItemType()"/>, возвращающего тип элемента DetailArray.
        /// </summary>
        [Fact]
        public void GetItemTypeTest()
        {
            try
            {
                var path = Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass);

                // Пошлем заведомо ложный путь чтобы все упало
                Information.GetItemType(typeof(InformationTestClass2), path + "ПАДАЙ!");
                Assert.True(false, "Assert.Fail");
            }
            catch (Exception)
            {
                // Тест должен свалиться на вызове метода, если зашел сюда - все хорошо.
            }

            // Входные параметры: тип данных: DetailClass, свойство: MasterClass.StringMasterProperty.
            var actual = Information.GetItemType(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.StringMasterProperty));

            // Ожидаемый результат: null.
            Assert.Equal(null, actual);

            // Входные параметры: тип данных: InformationTestClass2, свойство: InformationTestClass.stringPropertyForInformationTestClass.
            var actual1 = Information.GetItemType(typeof(InformationTestClass2), Information.ExtractPropertyPath<InformationTestClass2>(c => c.InformationTestClass.StringPropertyForInformationTestClass));

            // Ожидаемый результат: null.
            Assert.Equal(null, actual1);
        }

        /// <summary>
        /// Тест метода <see cref="GetCompatibleTypesForDetailProperty()"/>,  возвращающего типы, совместимые с детейловым свойством(по TypeUsage).
        /// </summary>
        [Fact]
        public void GetCompatibleTypesForDetailPropertyTest()
        {
            // Ожидаемый результат: массив, состоящий из типа данных DetailClass.
            Type[] expected = { typeof(DetailClass) };

            // Входные параметры: тип данных: DetailClass, свойства класса-мастера: MasterClass.DetailClass.
            var actual = Information.GetCompatibleTypesForDetailProperty(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));
            Assert.True(EquivalenceMethods.EqualTypeArrays(expected, actual));
        }

        /// <summary>
        /// Тест метода <see cref="GetPropertyDisableAutoViewing()"/>, определяющего является ли свойство автоматически включаемым в представления.
        /// </summary>
        [Fact]
        public void GetPropertyDisableAutoViewingTest()
        {
            // Входные параметры: тип данных: DetailClass, свойства класса-мастера: MasterClass.DetailClass.
            var actual = Information.GetPropertyDisableAutoViewing(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));

            // Ожидаемый результат: false.
            Assert.False(actual);
        }

        /// <summary>
        /// Тест метода <see cref="GetAllTypesFromView()"/>, возвращаюшего список типов из представления.
        /// </summary>
        [Fact]
        public void GetAllTypesFromViewTest()
        {
            var expected = Information.GetAllTypesFromView(new View());

            Assert.True(expected.Count == 0);

            expected = Information.GetAllTypesFromView(null);

            Assert.True(expected.Count == 0);

            expected = new List<Type> { typeof(string), typeof(int), typeof(NullableDateTime) };

            var actual = Information.GetAllTypesFromView(MasterClass.Views.Test_MasterClassE);

            Assert.True(EquivalenceMethods.ListEquals(expected, actual));
        }

        /// <summary>
        /// Тест метода <see cref="IsStoredType()"/>, проверяющего хранимый ли Class.
        /// </summary>
        [Fact]
        public void IsStoredTypeTest()
        {
            // Вызовем метод, послав в него класс со стереотипом [NotStored], должно вернуться false
            var typeIsStored = Information.IsStoredType(typeof(NotStoredClass));
            Assert.False(typeIsStored);
        }

        /// <summary>
        /// Тест метода <see cref="Information.GetAllTypesFromView(ExtendedView)"/>, возвращаюшего список типов элементов представления,
        /// если ExtendedView формируется некорректно.
        /// </summary>
        [Fact]
        public void GetAllTypesFromExtendedViewTest0()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                Information.GetAllTypesFromView(new ExtendedView(new View()));
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="Information.GetAllTypesFromView(ExtendedView)"/>, возвращаюшего список типов элементов представления.
        /// </summary>
        [Fact]
        public void GetAllTypesFromExtendedViewTest()
        {
            var expected = Information.GetAllTypesFromView(null);

            Assert.True(expected.Count == 0);

            expected = new List<Type> { typeof(string), typeof(InformationTestClass), typeof(InformationTestClass3), typeof(NullableDateTime) };

            var list = new ArrayList
                           {
                               new PseudoDetailInExtendedView(
                                   "Test_DetailClassE", typeof(DetailClass), string.Empty, string.Empty)
                           };

            list.AddRange(MasterClass.Views.MasterClassL.Properties);
            var actual = Information.GetAllTypesFromView(new ExtendedView(MasterClass.Views.MasterClassL, list));
            Assert.True(EquivalenceMethods.ListEquals(expected, actual));
        }

        /// <summary>
        /// Тест метода <see cref="Information.GetAllTypesFromView(ExtendedView)"/>, возвращаюшего список типов элементов представления.
        /// Передаём список свойств, отличный от списка свойств представления.
        /// </summary>
        [Fact]
        public void GetAllTypesFromExtendedViewTest2()
        {
            var expected = new List<Type> { typeof(string), typeof(NullableDateTime) };

            var list = new ArrayList
                           {
                               MasterClass.Views.MasterClassL.Properties[0],
                               new PseudoDetailInExtendedView(
                                   "Test_DetailClassE", typeof(DetailClass), string.Empty, string.Empty)
                           };
            list.AddRange(MasterClass.Views.MasterClassE.Details);

            var actual = Information.GetAllTypesFromView(new ExtendedView(MasterClass.Views.MasterClassL, list));

            Assert.True(EquivalenceMethods.ListEquals(expected, actual));
        }

        /// <summary>
        /// Тест метода <see cref="GetClassCaption()"/>, возвращающего заголовок для класса.
        /// </summary>
        [Fact]
        public void GetClassCaptionTest()
        {
            const string Expected = "InformationTestClass";

            // Входной параметр: тип даных: InformationTestClass
            var actual = Information.GetClassCaption(typeof(InformationTestClass));

            // Второй вызов идет, чтобы почитать данные из кеша.
            var actual0 = Information.GetClassCaption(typeof(InformationTestClass));

            var actual2 = Information.GetClassCaption(typeof(ClassWithCaptions));
            Assert.Equal("Класс пятый", actual2);

            // Ожидаемый результат: "InformationTestClass"
            Assert.Equal(Expected, actual);
            Assert.Equal(Expected, actual0);
        }

        /// <summary>
        /// Тест метода <see cref="GetExpressionForProperty()"/>, возвращающего выражения, указанные атрибутами <see cref="DataServiceExpressionAttribute"/> для свойства.
        /// </summary>
        [Fact]
        public void GetExpressionForPropertyTest()
        {
            // Пошлем свойство класса, в котором нет DataServiceExpression.
            var actual = Information.GetExpressionForProperty(
                typeof(DetailClass), Information.ExtractPropertyPath<DetailClass>(c => c.MasterClass.DetailClass));
            Assert.True(actual.Count == 0);

            // Пошлем свойство класса, в котором есть DataServiceExpression.
            var actual1 = Information.GetExpressionForProperty(
                typeof(InformationTestClass), Information.ExtractPropertyPath<InformationTestClass>(c => c.PublicStringProperty));
            Assert.True(actual1.Count == 1);
            Assert.True(actual1[typeof(MSSQLDataService)].ToString() == "TestDataServiceExpression");

            // Проверим наследование DataServiceExpression.
            var actual2 = Information.GetExpressionForProperty(typeof(InformationTestClassChild), Information.ExtractPropertyPath<InformationTestClassChild>(c => c.PublicStringProperty));
            Assert.True(actual2.Count == 1);
            Assert.True(actual2[typeof(MSSQLDataService)].ToString() == "TestDataServiceExpressionChild");
        }

        /// <summary>
        /// Тест метода <see cref="Information.CheckCompatibleStorageTypes"/>, проверяющий на совместимость хралищ у двух типов данных.
        /// </summary>
        [Fact]
        public void CheckCompatibleStorageTypesTest()
        {
            // Совместимые типы.
            Assert.True(Information.CheckCompatibleStorageTypes(typeof(MasterClass), typeof(InheritedMasterClass)));
            Assert.True(Information.CheckCompatibleStorageTypes(typeof(MasterClass), typeof(MasterClass)));

            // Несовместимые типы.
            Assert.True(!Information.CheckCompatibleStorageTypes(typeof(MasterUpdateObjectTest), typeof(AggregatorUpdateObjectTest)));
        }

        /// <summary>
        /// Специальный класс для теста, имеющий пустое значение.
        /// </summary>
        public class SpecialEmptyValueForTest : ISpecialEmptyValue
        {
            /// <summary>
            /// Пустое значение.
            /// </summary>
            /// <param name="value">
            /// Значение.
            /// </param>
            /// <returns>
            /// Оно пустое. <see cref="bool"/>.
            /// </returns>
            [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
            public bool IsEmptyValue(object value)
            {
                return true;
            }
        }

        /// <summary>
        /// Специально созданное для тестирования некоторых методов перечисление, состоящее из пустого и непустого значений.
        /// </summary>
        private enum SwappedEnum
        {
            [Caption("SwappedVal")]
            Val,

            [Caption("Val")]
            [EmptyEnumValue]
            SwappedVal
        }

        /// <summary>
        /// Специально созданное для тестирования некоторых методов перечисление, состоящее из трёх элементов с непустым заголовком, элемента с заголовоком, состоящим из пустой строки и элементом без заголовка.
        /// </summary>
        private enum CaseSensitiveEnum
        {
            [Caption("LAST DAY")]
            Year9998,

            [Caption("Last day")]
            Year9999,

            [Caption("CasEinSenSitiVe")]
            CaseInsensitive,

            [Caption("")]
            CaseINSENSITIVEVAL,

            CASEINSENSITIVEVAL
        }

        /// <summary>
        /// Пронумерованные годы.
        /// </summary>
        private enum NumberedYear
        {
            [Caption("2011")]
            Year2011,

            [Caption("2012")]
            Year2012,

            [Caption("2013")]
            Year2013,

            [Caption("")]
            Year2014
        }
    }
}