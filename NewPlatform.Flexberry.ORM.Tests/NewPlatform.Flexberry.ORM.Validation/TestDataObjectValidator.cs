namespace NewPlatform.Flexberry.ORM.Validation
{
    using System;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.KeyGen;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using NewPlatform.Flexberry.ORM.Validation.Exceptions;

    /// <summary>
    /// Тесты класса <see cref="DataObjectValidator"/>.
    /// </summary>
    public class TestDataObjectValidator
    {
        // TODO: написать по остальным типам.

        /// <summary>
        /// Язык задания ограничений.
        /// </summary>
        private readonly ExternalLangDef _languageDef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Проверка метода <see cref="DataObjectValidator.CheckObject"/>.
        /// Проверяем работу с полем типа <see cref="KeyGuid"/>.
        /// </summary>
        [Fact]
        public void TestDataObjectValidatorOwnGuidProperty()
        {
            // Arrange.
            Guid primaryKey = Guid.NewGuid();
            var fullTypesMainAgregator = new FullTypesMainAgregator()
            {
                __PrimaryKey = primaryKey,
            };
            Function trueFunction = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, Information.ExtractPropertyPath<DataObject>(x => x.__PrimaryKey)), primaryKey);
            Function falseFunction = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, Information.ExtractPropertyPath<DataObject>(x => x.__PrimaryKey)), Guid.Empty);

            // Act & Assert.
            Assert.True(DataObjectValidator.CheckObject(fullTypesMainAgregator, trueFunction));
            Assert.False(DataObjectValidator.CheckObject(fullTypesMainAgregator, falseFunction));
        }

        /// <summary>
        /// Проверка метода <see cref="DataObjectValidator.CheckObject"/>.
        /// Проверяем работу с OR.
        /// </summary>
        [Fact]
        public void TestDataObjectValidatorOr()
        {
            // Arrange.
            Guid primaryKey = Guid.NewGuid();
            Guid otherGuid = Guid.NewGuid();
            Guid other2Guid = Guid.Empty;
            Function withFirstFunction = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, Information.ExtractPropertyPath<DataObject>(x => x.__PrimaryKey)), primaryKey);
            Function withSecondFunction = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, Information.ExtractPropertyPath<DataObject>(x => x.__PrimaryKey)), otherGuid);
            Function withThirdFunction = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, Information.ExtractPropertyPath<DataObject>(x => x.__PrimaryKey)), other2Guid);

            var fullTypesMainAgregator = new FullTypesMainAgregator()
            {
                __PrimaryKey = primaryKey,
            };

            Function trueFunction = _languageDef.GetFunction(_languageDef.funcOR, withFirstFunction, withSecondFunction);
            Function true2Function = _languageDef.GetFunction(_languageDef.funcOR, withSecondFunction, withFirstFunction);
            Function falseFunction = _languageDef.GetFunction(_languageDef.funcOR, withSecondFunction, withThirdFunction);

            // Act & Assert.
            Assert.True(DataObjectValidator.CheckObject(fullTypesMainAgregator, trueFunction));
            Assert.True(DataObjectValidator.CheckObject(fullTypesMainAgregator, true2Function));
            Assert.False(DataObjectValidator.CheckObject(fullTypesMainAgregator, falseFunction));
        }

        /// <summary>
        /// Проверка метода <see cref="DataObjectValidator.CheckObject"/>.
        /// Проверяем работу с незагруженными свойствами.
        /// </summary>
        [Fact]
        public void TestDataObjectValidatorNotLoadedProperty()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Arrange.
                string fieldName = Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleString);
                Function notLoadedFunction = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, fieldName), "123");
                var fullTypesMainAgregator = new FullTypesMainAgregator();
                fullTypesMainAgregator.SetExistObjectPrimaryKey(Guid.NewGuid());
                fullTypesMainAgregator.PoleString = "123";

                // Act & Assert.
                Assert.True(fullTypesMainAgregator.GetStatus() != ObjectStatus.Created);
                Assert.False(fullTypesMainAgregator.GetLoadedProperties().Contains(fieldName));
                DataObjectValidator.CheckObject(fullTypesMainAgregator, notLoadedFunction);
            });
            Assert.IsType(typeof(UsedNotLoadedPropertyValidationException), exception);
        }
    }
}
