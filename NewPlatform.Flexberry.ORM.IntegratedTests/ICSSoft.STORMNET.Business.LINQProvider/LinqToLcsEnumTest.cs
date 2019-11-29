namespace NewPlatform.Flexberry.ORM.IntegratedTests.LINQProvider
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using ICSSoft.STORMNET.Business.LINQProvider.Tests;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using ExternalLangDef = ICSSoft.STORMNET.Windows.Forms.ExternalLangDef;

    /// <summary>
    /// Проверка перевода из Linq в LCS.
    /// </summary>
    public class LinqToLcsEnumTest
    {
        /// <summary>
        /// Язык задания ограничений.
        /// </summary>
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Вспомогательный класс, в котором есть поле системного перечислимого типа.
        /// </summary>
        private class TestClassWithEnum : DataObject
        {
            /// <summary>
            /// Поле системного перечислимого типа.
            /// </summary>
            private DayOfWeek poleEnum;

            /// <summary>
            /// Поле системного перечислимого типа.
            /// </summary>
            public DayOfWeek PoleEnum
            {
                get
                {
                    DayOfWeek result = this.poleEnum;
                    return result;
                }
                set
                {
                    this.poleEnum = value;
                }
            }

            /// <summary>
            /// Получение представления для данного вспомогательного типа.
            /// </summary>
            /// <returns>Сформированное представление.</returns>
            public static View GetView()
            {
                var view = new View() { DefineClassType = typeof(TestClassWithEnum), Name = "TestView" };
                view.AddProperty(Information.ExtractPropertyPath<TestClassWithEnum>(x => x.PoleEnum));
                return view;
            }
        }

        /// <summary>
        /// Проверка перевода Linq в lcs для Enum, где значение перечислимого типа задано в отдельной переменной.
        /// </summary>
        [Fact]
        public void TestEnumLinqToLcsWithVariable()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            PoleEnum poleEnum = PoleEnum.Attribute1;
            var predicate1 = (Expression<Func<FullTypesMainAgregator, bool>>)(o => o.PoleEnum == poleEnum);
            new Query<FullTypesMainAgregator>(testProvider).Where(predicate1).ToArray();

            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcEQ,
                        new VariableDef(ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleEnum)),
                        EnumCaption.GetCaptionFor(PoleEnum.Attribute1))
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка перевода Linq в lcs для Enum, где значение перечислимого типа задано прямо в Linq-выражении.
        /// Данный тест проходит после использования компилятора VS2015.
        /// При использовании компилятора VS2013 или VS2015 ServicePack 1 вызывается <see cref="NotSupportedException"/>.
        /// </summary>
        [Fact]
        public void TestEnumLinqToLcsWithoutVariable()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Arrange.
                var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
                var predicate1 = (Expression<Func<FullTypesMainAgregator, bool>>)(o => o.PoleEnum == PoleEnum.Attribute1);
                new Query<FullTypesMainAgregator>(testProvider).Where(predicate1).ToArray();

                Expression queryExpression = testProvider.InnerExpression;
                var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        ldef.GetFunction(
                            ldef.funcEQ,
                            new VariableDef(ldef.StringType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleEnum)),
                            EnumCaption.GetCaptionFor(PoleEnum.Attribute1))
                };

                // Act.
                // При компиляции в VS2013 или VS2015 ServicePack 1 данная строка вызовет NotSupportedException.
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

                // Assert.
                Assert.True(Equals(expected, actual));
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Проверка перевода Linq в lcs для перечисления системного типа.
        /// </summary>
        [Fact]
        public void TestSystemEnumLinqToLcsWithoutVariable()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                // Arrange.
                var testProvider = new TestLcsQueryProvider<TestClassWithEnum>();
                var predicate1 = (Expression<Func<TestClassWithEnum, bool>>)(o => o.PoleEnum == DayOfWeek.Saturday);
                new Query<TestClassWithEnum>(testProvider).Where(predicate1).ToArray();
                Expression queryExpression = testProvider.InnerExpression;

                // Act.
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, TestClassWithEnum.GetView());
            });
            Assert.IsType(typeof(FunctionalLanguageDef.NotFoundFunctionParametersException), exception);
        }
    }
}
