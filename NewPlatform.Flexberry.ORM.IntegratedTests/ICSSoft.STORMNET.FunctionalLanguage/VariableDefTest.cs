namespace ICSSoft.STORMNET.Tests.TestClasses.FunctionalLanguage
{
    using System;
    using System.Linq;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Проверка класса VariableDef.
    /// </summary>

    public class VariableDefTest
    {
        /// <summary>
        /// Проверка определения переменной в ограничении.
        /// </summary>
        
        [Fact]
        public void VariableDefTest1()
        {
            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            string objectStringView = "TestStirngView";
            string objectCaption = "TestObjectCaption";

            VariableDef variableDef = new VariableDef(langDef.StringType, objectStringView, objectCaption);

            Assert.True(variableDef.PrimaryKeyIsUnique);
            Assert.Equal(objectStringView, variableDef.StringedView);
            Assert.Equal(objectCaption, variableDef.Caption);
        }

        /// <summary>
        /// Проверка определения переменной в ограничении.
        /// </summary>
        
        [Fact]
        public void VariableDefTest2()
        {
            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            string objectStringView = "TestStirngView";

            VariableDef variableDef = new VariableDef(langDef.StringType, objectStringView);

            Assert.True(variableDef.PrimaryKeyIsUnique);
            Assert.Equal(objectStringView, variableDef.StringedView);
        }

        /// <summary>
        /// Проверка определения переменной в ограничении.
        /// </summary>
        
        [Fact]
        public void VariableDefTest3()
        {
            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            string property = "Name";
            string objCaption = "TestObjectCaption";

            VariableDef variableDef = new VariableDef(typeof(TypeUsageProviderTestClass), property, objCaption, langDef);

            Assert.True(variableDef.PrimaryKeyIsUnique);
            Assert.Equal(property, variableDef.StringedView);
            Assert.Equal(objCaption, variableDef.Caption);
        }

        /// <summary>
        /// Проверка определения переменной в ограничении.
        /// </summary>
        
        [Fact]
        public void VariableDefTest4()
        {
            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            string property = "Name";

            VariableDef variableDef = new VariableDef(typeof(TypeUsageProviderTestClass), property, langDef);

            Assert.True(variableDef.PrimaryKeyIsUnique);
            Assert.Equal(property, variableDef.StringedView);
        }

        /// <summary>
        /// Проверка сериализации.
        /// </summary>
        
        [Fact]
        public void VariableDefToSimpleValueTest()
        {
            Assert.True(VariableDefToSimpleValue(null, string.Empty));
            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            Assert.True(VariableDefToSimpleValue(langDef.StringType, "String"));
            Assert.True(VariableDefToSimpleValue(langDef.NumericType, "Numeric"));
            Assert.True(VariableDefToSimpleValue(langDef.BoolType, "Boolean"));
            Assert.True(VariableDefToSimpleValue(langDef.DateTimeType, "DateTime"));
            Assert.True(VariableDefToSimpleValue(langDef.GuidType, "Guid"));
        }

        /// <summary>
        /// Проверка десериализации.
        /// </summary>
        
        [Fact]
        public void VariableDefFromSimpleValueTest()
        {
            Assert.True(VariableDefFromSimpleValue(String.Empty));
            Assert.True(VariableDefFromSimpleValue("System.Int16"));
            Assert.True(VariableDefFromSimpleValue("Int32"));
            Assert.True(VariableDefFromSimpleValue("NullableDateTime"));
            Assert.True(VariableDefFromSimpleValue("NullableDecimal"));
            Assert.True(VariableDefFromSimpleValue("NullableInt"));
            Assert.True(VariableDefFromSimpleValue("String"));
            Assert.True(VariableDefFromSimpleValue("Boolean"));
            Assert.True(VariableDefFromSimpleValue("DateTime"));
            Assert.True(VariableDefFromSimpleValue("Numeric"));
            Assert.True(VariableDefFromSimpleValue("KeyGuid"));
        }

        /// <summary>
        /// Проверка исключения при десериализации.
        /// </summary>
        
        [Fact]
        public void VariableDefFromSimpleValueExceptionTest()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                object testData = new string[] { "UnknownType", "TestStirngView", "TestObjectCaption" };

                SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
                VariableDef variableDef = new VariableDef();

                variableDef.FromSimpleValue(testData, langDef);
            });
            Assert.IsType(typeof(Exception), exception);
        }

        /// <summary>
        /// Проверка логики для VariableDefToSimpleValueTest.
        /// </summary>
        /// <param name="objType">
        /// Проверяемый тип.
        /// </param>
        /// <param name="typeName">
        /// Название типа.
        /// </param>
        /// <returns>
        /// Возвращает true если тест прошел.
        /// </returns>
        private bool VariableDefToSimpleValue(ObjectType objType, string typeName)
        {
            var result = false;

            string objectStringView = "TestStirngView";
            string objectCaption = "TestObjectCaption";
            VariableDef variableDef = new VariableDef(objType, objectStringView, objectCaption);
            var resultToSimpleValue = (string[])variableDef.ToSimpleValue();

            if (resultToSimpleValue.Length == 3 &&
                resultToSimpleValue[0] == typeName &&
                resultToSimpleValue[1] == objectStringView &&
                resultToSimpleValue[2] == objectCaption)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Проверка логики для VariableDefFromSimpleValueTest.
        /// </summary>
        /// <param name="objType">
        /// Проверяемый тип.
        /// </param>
        /// <returns>
        /// Возвращает true если тест прошел.
        /// </returns>
        private bool VariableDefFromSimpleValue(string objType)
        {
            bool result = false;

            object testData = new string[] { objType, "TestStirngView", "TestObjectCaption" };

            SQLWhereLanguageDef langDef = SQLWhereLanguageDef.LanguageDef;
            VariableDef variableDef = new VariableDef();

            variableDef.FromSimpleValue(testData, langDef);

            string findType = String.Empty;
            switch (objType)
            {
                case "":
                    findType = "String";
                    break;

                // различная обработка NumericType в методах ToSimpleValue() сериализуем в "Numeric"
                // а в методе FromSimpleValue() десериализуем в  "Decimal" (инф добавил в отчет).
                case "Numeric":
                    findType = "Decimal";
                    break;

                // Различие десериализации в FromSimpleValue() "KeyGuid" -> "Guid" и 
                // "keyguid" -> "KeyGuid". В одном методе два оператора switch (инф добавил в отчет).
                case "KeyGuid":
                    findType = "Guid";
                    break;

                default:
                    findType = objType.Split('.').Last();
                    break;
            }

            var findAddType = langDef.Types.Cast<ObjectType>()
                .FirstOrDefault(i => i.NetCompatibilityType.Name == findType);

            if (findAddType != null)
            {
                result = true;
            }

            return result;
        }
    }
}
