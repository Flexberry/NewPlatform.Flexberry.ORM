namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
    using System;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using Xunit;

    /// <summary>
    /// Проверка класса ObjectType.
    /// </summary>
    public class ObjectTypeTest
    {
        /// <summary>
        /// Проверка совпадения типов.
        /// </summary>
        [Fact]
        public void ObjectTypeCompatWithEqualTest()
        {
            var testObject = new ObjectType("objStringedView", "objCaption", typeof(Int32));

            Assert.True(testObject.CompatWithEqual(testObject), "Сравнение с самим собой.");
            Assert.True(ObjectTypeCompatWithEqual(testObject, typeof(Int32)), "Сравнение одинаковых типов.");
            Assert.False(ObjectTypeCompatWithEqual(testObject, typeof(string)), "Сравнение разных типов.");
            Assert.True(ObjectTypeCompatWithEqual(testObject, typeof(Int32)), "Сравнение с Nullable аналагом.");
            Assert.False(ObjectTypeCompatWithEqual(testObject, typeof(bool?)), "Сравнение с Nullable другого типа.");
        }

        /// <summary>
        /// Проверка метода ValueToSimpleValue значение в простое значение.
        /// </summary>
        [Fact]
        public void ObjectTypeValueToSimpleValueTest()
        {
            var testObject = new ObjectType("objStringedView", "objCaption", typeof(Int32));
            Assert.False((bool)testObject.ValueToSimpleValue(false), "delegate == null");

            testObject.SimplificationValue = SimplificationValue;

            Assert.True((bool)testObject.ValueToSimpleValue(false), "delegate != null");

            testObject.SimplificationValue = SimplificationValueExc;
            Assert.False((bool)testObject.ValueToSimpleValue(false), "process exception");
        }

        /// <summary>
        /// Проверка метода SimpleValueToValue простое значение в значение.
        /// </summary>
        [Fact]
        public void ObjectTypeSimpleValueToValueTest()
        {
            var testObject = new ObjectType("objStringedView", "objCaption", typeof(Int32));
            Assert.False((bool)testObject.SimpleValueToValue(false), "delegate == null");

            testObject.UnSimplificationValue = SimplificationValue;
            Assert.True((bool)testObject.SimpleValueToValue(false), "delegate != null");

            // TODO: обработать после выполнения задачи 4014
            // testObject.UnSimplificationValue = SimplificationValueExc;
            // Assert.False((bool)testObject.SimpleValueToValue(false), "process exception");
        }

        /// <summary>
        /// Проверка логики для ObjectTypeCompatWithEqualTest.
        /// </summary>
        /// <param name="obj">
        /// Объект с которым производится сравнение.
        /// </param>
        /// <param name="type">
        /// Сравниваемый Type, который сравнивается с ObjectType.
        /// </param>
        /// <returns>
        /// Возращает результат сравнения <see cref="bool"/>.
        /// </returns>
        private bool ObjectTypeCompatWithEqual(ObjectType obj, Type type)
        {
            var testObject = new ObjectType("objStringedView1", "objCaption1", type);
            return obj.CompatWithEqual(testObject);
        }

        /// <summary>
        /// Проверка логики для ObjectTypeValueToSimpleValueTest и ObjectTypeSimpleValueToValueTest.
        /// </summary>
        /// <param name="value">
        /// Любой объект.
        /// </param>
        /// <returns>
        /// Всегда возвращается true <see cref="object"/>.
        /// </returns>
        private object SimplificationValue(object value)
        {
            return true;
        }

        /// <summary>
        /// Проверка логики для ObjectTypeValueToSimpleValueTest и ObjectTypeSimpleValueToValueTest.
        /// </summary>
        /// <param name="value">
        /// Любой объект.
        /// </param>
        /// <returns>
        /// Всегда генерируется исключение <see cref="object"/>.
        /// </returns>
        /// <exception cref="Exception"> Всегда генерируется исключение.
        /// </exception>
        private object SimplificationValueExc(object value)
        {
            throw new Exception();
        }
    }
}
