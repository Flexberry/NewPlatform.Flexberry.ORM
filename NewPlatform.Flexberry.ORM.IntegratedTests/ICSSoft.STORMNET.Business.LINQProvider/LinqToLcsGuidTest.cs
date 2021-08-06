namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using Windows.Forms;
    using FunctionalLanguage;

    /// <summary>
    /// Тесты на работу провайдера со свойствами типа Guid.
    /// </summary>
    public class LinqToLcsGuidTest
    {
        /// <summary>
        /// Язык задания ограничений.
        /// </summary>
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Проверяем ситуацию, когда собственное свойство типа KeyGuid.
        /// </summary>
        [Fact]
        public void TestUseKeyGuidProperty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider)
                .Where(o =>
                    o.Ключ == "{72FCA622-A01E-494C-BE1C-0821178594FB}"
                    && o.Порода.__PrimaryKey.ToString() == "{f6af2f25-1eca-4b62-8094-3284be51d73e}")
                 .ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Information.GetView("КошкаGuid", typeof(Кошка)));

            Assert.Equal(
                "AND ( = ( Ключ {72FCA622-A01E-494C-BE1C-0821178594FB} )  = ( Порода {f6af2f25-1eca-4b62-8094-3284be51d73e} ) )",
                actual.LimitFunction.ToString());
        }

        /// <summary>
        /// Проверяем ситуацию, когда свойство мастера типа KeyGuid.
        /// </summary>
        [Fact]
        public void TestUseMasterKeyGuidProperty()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider)
                .Where(o =>
                    o.Порода.Ключ == "{72FCA622-A01E-494C-BE1C-0821178594FB}"
                    && o.Порода.__PrimaryKey.ToString() == "{f6af2f25-1eca-4b62-8094-3284be51d73e}"
                    && o.Порода.Название == "SomeName")
                 .ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Information.GetView("КошкаGuid", typeof(Кошка)));
            Assert.Equal(
                "AND ( AND ( = ( Порода.Ключ {72FCA622-A01E-494C-BE1C-0821178594FB} )  = ( Порода {f6af2f25-1eca-4b62-8094-3284be51d73e} ) )  = ( Порода.Название SomeName ) )",
                actual.LimitFunction.ToString());
        }

        /// <summary>
        /// Проверяем ситуацию, когда свойство мастера типа <see cref="Guid"/>.
        /// </summary>
        [Fact]
        public void TestUseGuidProperty()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            Guid eqGuid = new Guid("{72FCA622-A01E-494C-BE1C-0821178594FB}");

            new Query<FullTypesMainAgregator>(testProvider).Where(o => o.PoleGuid == eqGuid).ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.GuidType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleGuid)),
                        eqGuid),
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем ситуацию, когда свойство мастера типа <see cref="Guid?"/> и сравнивается с непустым значением.
        /// </summary>
        [Fact]
        public void TestUseNullableGuidProperty()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            Guid? eqGuid = new Guid("{72FCA622-A01E-494C-BE1C-0821178594FB}");

            new Query<FullTypesMainAgregator>(testProvider).Where(o => o.PoleNullGuid == eqGuid).ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.GuidType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleNullGuid)),
                        eqGuid),
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем ситуацию, когда свойство мастера типа <see cref="Guid?"/> и сравнивается с пустым значением.
        /// </summary>
        [Fact]
        public void TestUseNullableGuidPropertyAsNull()
        {
            // Arrange.
            var testProvider = new TestLcsQueryProvider<FullTypesMainAgregator>();
            new Query<FullTypesMainAgregator>(testProvider).Where(o => o.PoleNullGuid == null).ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcIsNull,
                        new VariableDef(this.ldef.GuidType, Information.ExtractPropertyPath<FullTypesMainAgregator>(x => x.PoleNullGuid))),
            };

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, FullTypesMainAgregator.Views.FullView);

            // Assert.
            Assert.True(Equals(expected, actual));
        }
    }
}
