namespace NewPlatform.Flexberry.ORM.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AdvLimit.ExternalLangDef;

    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;

    using Xunit;
    using ICSSoft.STORMNET;

    /// <summary>
    /// Тесты, которые проверяют расширение представления по функции.
    /// Используемые в функции свойства должны попасть в представление.
    /// </summary>
    public class EnrichViewWithPropertiesUsedInFunctionTest
    {
        /// <summary>
        /// Описание языка lcs, используемое в тестах.
        /// </summary>
        private readonly ExternalLangDef langdef = new ExternalLangDef();

        [Fact]
        public void NullFunctionTest()
        {
            var expectedPropertiesUsedInFunction = new List<string>();
            var propertiesUsedInFunction = new List<string>();
            ViewPropertyAppender.FindPropertiesUsedInFunction(null, expectedPropertiesUsedInFunction, new List<ViewPropertyAppender.DetailVariableDefContainer>());
            var intersection = expectedPropertiesUsedInFunction.Intersect(propertiesUsedInFunction).ToList();

            Assert.Equal(expectedPropertiesUsedInFunction.Count, intersection.Count);
        }

        [Fact]
        public void SimpleDoubledPropertyTest()
        {
            var function = langdef.GetFunction(
                langdef.funcAND,
                langdef.GetFunction(langdef.funcEQ, langdef.GetFunction(langdef.funcYearPart, new VariableDef(langdef.DateTimeType, "ДатаВыдачи")), "2012"),
                langdef.GetFunction(langdef.funcEQ, langdef.GetFunction(langdef.funcMonthPart, new VariableDef(langdef.DateTimeType, "ДатаВыдачи")), "12"));

            var expectedPropertiesUsedInFunction = new List<string> { "ДатаВыдачи" };

            var propertiesUsedInFunction = new List<string>();
            ViewPropertyAppender.FindPropertiesUsedInFunction(function, propertiesUsedInFunction, new List<ViewPropertyAppender.DetailVariableDefContainer>());

            var intersection = expectedPropertiesUsedInFunction.Intersect(propertiesUsedInFunction).ToList();

            Assert.Equal(expectedPropertiesUsedInFunction.Count, intersection.Count);
        }

        /// <summary>
        /// Проверка, что если выполняется ограничение на детейл, то детейловые свойства не вылезут в основное представление.
        /// </summary>
        [Fact]
        public void DetailInFunctionTest()
        {
            var dvd = new DetailVariableDef();
            dvd.ConnectMasterPorp = "Берлога";
            dvd.OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey };
            dvd.View = Information.GetView("МедведьE", typeof(Медведь));
            dvd.Type = langdef.GetObjectType("Details");

            var function = langdef.GetFunction(langdef.funcExist, dvd, langdef.GetFunction(langdef.funcEQ, new VariableDef(langdef.GuidType, "Наименование"), string.Empty));

            var expectedPropertiesUsedInFunction = new List<string> { "Наименование" };

            var propertiesUsedInFunction = new List<string>();
            var detailList = new List<ViewPropertyAppender.DetailVariableDefContainer>();
            ViewPropertyAppender.FindPropertiesUsedInFunction(function, propertiesUsedInFunction, detailList);
            Assert.Equal(0, propertiesUsedInFunction.Count);
            Assert.Equal(1, detailList.Count);

            var intersection = expectedPropertiesUsedInFunction.Intersect(detailList[0].DetailVariablesList).ToList();
            Assert.Equal(expectedPropertiesUsedInFunction.Count, intersection.Count);
        }

        /// <summary>
        /// Проверка корректного добавления вычислимых свойств.
        /// </summary>
        [Fact]
        public void AddPropertyByLimitFunctionWithExpression()
        {
            var ds = new MSSQLDataService();
            var function = langdef.GetFunction(langdef.funcEQ, new VariableDef(langdef.GuidType, "МедведьСтрокой"), "Бум");

            var view = ViewPropertyAppender.GetViewWithPropertiesUsedInFunction(Медведь.Views.МедведьShort, function, ds);

            var expectedPropertiesUsedInFunction = new List<string> { "МедведьСтрокой", "ПорядковыйНомер", "Мама.ЦветГлаз" };
            var propertiesUsedInFunction = view.Properties.Select(p => p.Name).ToList();

            var intersection = expectedPropertiesUsedInFunction.Intersect(propertiesUsedInFunction).ToList();

            Assert.Equal(expectedPropertiesUsedInFunction.Count, intersection.Count);
        }

        /// <summary>
        /// Проверка <see cref="ViewPropertyAppender.EnrichDetailViewInLimitFunction" />: метод добавляет в представления
        /// детейла недостающие свойства.
        /// </summary>
        [Fact]
        public void EnrichDetailViewTest()
        {
            var ds = new MSSQLDataService();
            var dvd = new DetailVariableDef();
            dvd.ConnectMasterPorp = Information.ExtractPropertyPath<Выплаты>(x => x.Кредит1);
            dvd.OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey };
            dvd.View = new View { DefineClassType = typeof(Выплаты) };
            dvd.Type = langdef.GetObjectType("Details");

            var function = langdef.GetFunction(
                langdef.funcExist,
                dvd,
                langdef.GetFunction(
                    langdef.funcAND,
                    langdef.GetFunction(langdef.funcEQ, new VariableDef(langdef.DateTimeType, Information.ExtractPropertyPath<Выплаты>(x => x.ДатаВыплаты)), DateTime.Now),
                    langdef.GetFunction(langdef.funcEQ, new VariableDef(langdef.NumericType, Information.ExtractPropertyPath<Выплаты>(x => x.СуммаВыплаты)), 123)));

            ViewPropertyAppender.EnrichDetailViewInLimitFunction(function, ds);
            Assert.Equal(2, dvd.View.Properties.Count());
            Assert.Equal(Information.ExtractPropertyPath<Выплаты>(x => x.ДатаВыплаты), dvd.View.Properties[0].Name);
            Assert.Equal(Information.ExtractPropertyPath<Выплаты>(x => x.СуммаВыплаты), dvd.View.Properties[1].Name);
        }
    }
}