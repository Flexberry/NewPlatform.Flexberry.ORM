namespace NewPlatform.Flexberry.ORM.IntegratedTests.FunctionalLanguage
{
    using System;
    using System.Linq;
    using Xunit;

    using System.Configuration;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using NewPlatform.Flexberry.ORM.Tests;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using ICSSoft.STORMNET;

    /// <summary>
    /// Тестирование разных вариантов загрузки char из БД.
    /// </summary>
    public class LoadCharTest : BaseIntegratedTest
    {
        /// <summary>
        /// Язык задания ограничений.
        /// </summary>
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Конструктор класса теста.
        /// Задает все необходимые параметры для генерации временных баз.
        /// </summary>
        public LoadCharTest() : base("LoadChar")
        {
        }

        /// <summary>
        /// Проверка, что LinqProvider и lcs корректно и одинаково работают с символьным типом.
        /// </summary>
        [Fact]
        public void TestLoadChar()
        {
            // Arrange.
            char testChar = '1';
            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), FullTypesMainAgregator.Views.FullView);

            // 49 - это код символа '1'.
            lcs.LimitFunction = this.ldef.GetFunction(
                this.ldef.funcEQ,
                new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleChar)),
                49);

            LoadingCustomizationStruct lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), FullTypesMainAgregator.Views.FullView);
            lcs2.LimitFunction = this.ldef.GetFunction(
                this.ldef.funcEQ,
                new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleChar)),
                testChar);

            foreach (IDataService dataService in DataServices)
            {
                var testObject = new FullTypesMainAgregator() { PoleChar = testChar, FullTypesMaster1 = new FullTypesMaster1() };
                dataService.UpdateObject(testObject);

                // Act.
                int count = dataService.GetObjectsCount(lcs);
                int count2 = dataService.GetObjectsCount(lcs2);
                int linqCount = dataService.Query<FullTypesMainAgregator>(FullTypesMainAgregator.Views.FullView.Name).Count(x => x.PoleChar == testChar);
                int linq2Count = dataService.Query<FullTypesMainAgregator>(FullTypesMainAgregator.Views.FullView.Name).Count(x => x.PoleChar == '1');

                // Assert.
                Assert.Equal(1, count);
                Assert.Equal(count, count2);
                Assert.Equal(count, linqCount);
                Assert.Equal(count, linq2Count);
            }
        }

        /// <summary>
        /// Проверка, что LinqProvider и lcs корректно и одинаково работают с nullable-символьным типом.
        /// </summary>
        [Fact]
        public void TestLoadNullChar()
        {
            // Arrange.
            char testChar = '1';
            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), FullTypesMainAgregator.Views.FullView);

            // 49 - это код символа '1'.
            lcs.LimitFunction = this.ldef.GetFunction(
                this.ldef.funcEQ,
                new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleNullChar)),
                49);

            LoadingCustomizationStruct lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(FullTypesMainAgregator), FullTypesMainAgregator.Views.FullView);
            lcs2.LimitFunction = this.ldef.GetFunction(
                this.ldef.funcEQ,
                new VariableDef(this.ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleNullChar)),
                testChar);

            foreach (IDataService dataService in DataServices)
            {
                var testObject = new FullTypesMainAgregator() { PoleNullChar = '1', FullTypesMaster1 = new FullTypesMaster1() };
                dataService.UpdateObject(testObject);

                // Act.
                int count = dataService.GetObjectsCount(lcs);
                int count2 = dataService.GetObjectsCount(lcs2);
                int linqCount = dataService.Query<FullTypesMainAgregator>(FullTypesMainAgregator.Views.FullView.Name).Count(x => x.PoleNullChar == testChar);
                int linq2Count = dataService.Query<FullTypesMainAgregator>(FullTypesMainAgregator.Views.FullView.Name).Count(x => x.PoleNullChar == '1');

                // Assert.
                Assert.Equal(1, count);
                Assert.Equal(count, count2);
                Assert.Equal(count, linqCount);
                Assert.Equal(count, linq2Count);
            }
        }
    }
}
