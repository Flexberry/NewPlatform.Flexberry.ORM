namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Exceptions;
    using Xunit;

    /// <summary>
    /// Тесты для класса <see cref="EnumCaption"/>.
    /// </summary>
    
    public class EnumCaptionTest
    {
        /// <summary>
        /// Тест выпадения исключения ArgumentException в методе EnumCaptionNotExistTest() в случае неверного заголовка элемента перечисления.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForNotExistTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Arrange.
                Type enumType = typeof(NumberedYear);

                // Act.
                EnumCaption.GetValueFor("notexist", enumType);

                // Assert. 
                // Ожидаем исключения.
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor"/> с передачей <c>null</c> вместо первого аргумента.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForNullTest()
        {
            // Arrange.
            Type enumType = typeof(NumberedYear);

            // Act.
            object actual = EnumCaption.GetValueFor(null, enumType);
            
            // Assert.
            Assert.Equal(NumberedYear.Year2014, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Передаём пустую строку в качестве заголовка.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForEmptyTest()
        {
            // Arrange.
            Type enumType = typeof(NumberedYear);

            // Act.
            // Входные параметры: пустая строка, тип данных NumberedYear.
            object actual = EnumCaption.GetValueFor(string.Empty, enumType);
            
            // Assert.
            Assert.Equal(NumberedYear.Year2014, actual);
        }
        
        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку.  Проверка числа в качестве заголовка.     
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForNumberTest()
        {
            // Arrange.
            Type enumType = typeof(NumberedYear);

            // Act.
            // Входные параметры:  заголовок элемента перечисления "2013", тип элемента перечисления.
            object actual = EnumCaption.GetValueFor("2013", enumType);

            // Assert.
            Assert.Equal(NumberedYear.Year2013, actual);
        }

        /// <summary>
        /// Тест выпадения исключения ArgumentException когда Enum.Parse вернул 1.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForPositionNumberTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Arrange.
                Type enumType = typeof(NumberedYear);

                // Act.
                // Enum.Parse вернул бы 1ый элемент enum, а GetValueFor должен бросить exception.
                EnumCaption.GetValueFor("1", enumType);

                // Assert. 
                // Ожидаем исключения.
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Проверяем перечисление на чувствительность к регистру в заголовке.     
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForCaseInsensitivePriorityTest()
        {
            // Arrange.
            Type enumType = typeof(CaseSensitiveEnum);

            // Act.
            // Входные параметры:  заголовок элемента перечисления "LAST DAY", тип элемента перечисления.
            object actual = EnumCaption.GetValueFor("LAST DAY", enumType);

            // Assert. 
            Assert.Equal(CaseSensitiveEnum.Year9998, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Проверяем перечисление на чувствительность к регистру в заголовке.   
        /// </summary>       
        [Fact]
        public void EnumCaptionGetValueForCaseInsensitiveTest()
        {
            // Arrange.
            Type enumType = typeof(CaseSensitiveEnum);

            // Act.
            // Входные параметры:  заголовок элемента перечисления "Last day", тип элемента перечисления.
            object actual = EnumCaption.GetValueFor("Last day", enumType);

            // Assert. 
            Assert.Equal(CaseSensitiveEnum.Year9999, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Проверяем перечисление на чувствительность к регистру в заголовке.       
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForCaseInsensitive2Test()
        {
            // Arrange.
            Type enumType = typeof(CaseSensitiveEnum);

            // Act.
            // Входные параметры:  заголовок элемента перечисления "CaseINSENSITIVEVAL", тип элемента перечисления.
            object actual = EnumCaption.GetValueFor("caseinsensitive", enumType);

            // Assert. 
            Assert.Equal(CaseSensitiveEnum.CaseInsensitive, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Проверяем перечисление на чувствительность к регистру в заголовке.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForCaseInsensitive3Test()
        {
            // Arrange.
            Type enumType = typeof(CaseSensitiveEnum);

            // Act.
            // Входные параметры:  заголовок элемента перечисления "CaseINSENSITIVEVAL", тип элемента перечисления.
            object actual = EnumCaption.GetValueFor("CaseINSENSITIVEVAL", enumType);
            
            // Assert. 
            Assert.Equal(CaseSensitiveEnum.CaseINSENSITIVEVAL, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Проверяем перечисление на чувствительность к регистру в заголовке.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForCaseInsensitive4Test()
        {
            // Arrange.
            Type enumType = typeof(CaseSensitiveEnum);

            // Act.
            // Входные параметры:  заголовок элемента перечисления "CaseINSENSITIVEVAL", тип элемента перечисления.
            object actual = EnumCaption.GetValueFor("CASEINSENSITIVEVAL", enumType);

            // Assert. 
            Assert.Equal(CaseSensitiveEnum.CASEINSENSITIVEVAL, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Случай, когда заголовки и значения противоположны друг другу.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForSwappedCaptionTest()
        {
            // Arrange.
            Type enumType = typeof(SwappedEnum);

            // Act.
            // Входные параметры: заголовок элемента перечисления "SwappedVal", тип элемента перечисления.
            object actual = EnumCaption.GetValueFor("SwappedVal", enumType);
            
            // Assert. 
            Assert.Equal(SwappedEnum.Val, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Случай, когда заголовки и значения противоположны друг другу.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForNotEnumTest()
        {
            // Arrange.
            Type enumType = typeof(DateTime);

            // Act.
            object actual = EnumCaption.GetValueFor("Now", enumType);

            // Assert. 
            Assert.Null(actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Случай, когда вместо заголовка передаётся строковое значение с различиями в регистре.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForValueCaseInsensitiveTest()
        {
            // Arrange.
            Type enumType = typeof(NumberedYear);

            // Act.
            object actual = EnumCaption.GetValueFor("YeAr2012", enumType);

            // Assert. 
            Assert.Equal(NumberedYear.Year2012, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetValueFor(string,System.Type)"/>, позволяющего получить enum-значение по заголовку. Случай, когда вместо заголовка передаётся строковое значение.
        /// </summary>
        [Fact]
        public void EnumCaptionGetValueForValueTest()
        {
            // Arrange.
            Type enumType = typeof(NumberedYear);

            // Act.
            object actual = EnumCaption.GetValueFor("Year2012", enumType);

            // Assert. 
            Assert.Equal(NumberedYear.Year2012, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.TryGetValueFor{TEnum}"/> с успешным завершением.
        /// </summary>
        [Fact]
        public void EnumCaptionTryGetValueForTestPositive()
        {
            // Arrange.
            NumberedYear actual;

            // Act.
            bool result = EnumCaption.TryGetValueFor("2013", out actual);

            // Assert.
            Assert.True(result);
            Assert.Equal(NumberedYear.Year2013, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.TryGetValueFor{TEnum}"/> с неудачной попыткой получения значения.
        /// </summary>
        [Fact]
        public void EnumCaptionTryGetValueForTestNegative()
        {
            // Arrange.
            NumberedYear actual;

            // Act.
            bool result = EnumCaption.TryGetValueFor("20-13", out actual);

            // Assert.
            Assert.False(result);
            Assert.Equal(default(NumberedYear), actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetCaptionFor"/> с удачной попыткой получения значения.
        /// </summary>
        [Fact]
        public void EnumCaptionGetCaptionForTest()
        {
            // Arrange.

            // Act.
            string actual = EnumCaption.GetCaptionFor(NumberedYear.Year2012);

            // Assert.
            Assert.Equal("2012", actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetCaptionFor"/> с удачной попыткой получения значения.
        /// </summary>
        [Fact]
        public void EnumCaptionGetCaptionForEmptyStringTest()
        {
            // Arrange.

            // Act.
            string actual = EnumCaption.GetCaptionFor(NumberedYear.Year2014);

            // Assert.
            Assert.Equal(string.Empty, actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetCaptionFor"/> с удачной попыткой получения значения.
        /// </summary>
        [Fact]
        public void EnumCaptionGetCaptionForWithoutCaptionTest()
        {
            // Arrange.

            // Act.
            string actual = EnumCaption.GetCaptionFor(CaseSensitiveEnum.CASEINSENSITIVEVAL);

            // Assert.
            Assert.Equal("CASEINSENSITIVEVAL", actual);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetCaptionFor"/>. Передаём <c>null</c> в качестве параметра.
        /// </summary>
        [Fact]
        public void EnumCaptionGetCaptionForNullTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Arrange.

                // Act.
                EnumCaption.GetCaptionFor(null);

                // Assert.
                // Ожидаем исключения.
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Тест метода <see cref="EnumCaption.GetCaptionFor"/>. Передаём в качестве параметра не перечислимый тип.
        /// </summary>
        [Fact]
        public void EnumCaptionGetCaptionForNotEnumTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Arrange.
                SimpleDataObject simpleDataObject = new SimpleDataObject();

                // Act.
                EnumCaption.GetCaptionFor(simpleDataObject);

                // Assert.
                // Ожидаем исключения.
            });
            Assert.IsType(typeof(NotEnumTypeException), exception);
        }
    }
}
