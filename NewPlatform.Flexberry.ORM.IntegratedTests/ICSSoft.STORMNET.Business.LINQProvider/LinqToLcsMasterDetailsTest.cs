namespace ICSSoft.STORMNET.Business.LINQProvider
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.Business.LINQProvider.Tests;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Test LinqProvider for limitations by master details like <see cref="MasterDetailsLimitTest.MasterDetailsLimitationTest"/>.
    /// </summary>
    public class LinqToLcsMasterDetailsTest
    {
        /// <summary>
        /// Test LinqProvider for limitations by master details like <see cref="MasterDetailsLimitTest.MasterDetailsLimitationTest"/>.
        /// </summary>
        [Fact]
        public void GetLcsTestAny()
        {
            object лес1Pk = Guid.NewGuid();

            View view = new View();
            view.DefineClassType = typeof(Блоха);
            view.AddProperty(Information.ExtractPropertyPath<Блоха>(б => б.Кличка));
            view.AddProperty(Information.ExtractPropertyPath<Блоха>(б => б.МедведьОбитания));
            view.AddProperty(Information.ExtractPropertyPath<Блоха>(б => б.МедведьОбитания.ПорядковыйНомер));

            LoadingCustomizationStruct expected = LoadingCustomizationStruct.GetSimpleStruct(typeof(Блоха), view);

            // Выберем всех блох, чей медведь живёт в берлоге 1.
            DetailVariableDef dvd = new DetailVariableDef();
            dvd.ConnectMasterPorp = Information.ExtractPropertyPath<Берлога>(б => б.Медведь);
            dvd.OwnerConnectProp = new string[] { Information.ExtractPropertyPath<Блоха>(б => б.МедведьОбитания) };
            View viewDetail = new View();
            viewDetail.DefineClassType = typeof(Берлога);
            viewDetail.AddProperty(Information.ExtractPropertyPath<Берлога>(б => б.Наименование));
            viewDetail.AddProperty(Information.ExtractPropertyPath<Берлога>(б => б.Медведь));
            viewDetail.AddProperty(Information.ExtractPropertyPath<Берлога>(б => б.ЛесРасположения));

            dvd.View = viewDetail;
            ExternalLangDef ldef = ExternalLangDef.LanguageDef;
            dvd.Type = ldef.DetailsType;
            Function detailsLimitName = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, Information.ExtractPropertyPath<Берлога>(б => б.Наименование)), "Берлога 1");
            Function detailsLimitForest = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.GuidType, Information.ExtractPropertyPath<Берлога>(б => б.ЛесРасположения)), лес1Pk);
            Function detailsLimit = ldef.GetFunction(ldef.funcAND, detailsLimitName, detailsLimitForest);
            expected.LimitFunction = ldef.GetFunction(ldef.funcExist, dvd, detailsLimit);

            var testProvider = new TestLcsQueryProvider<Блоха>();

            new Query<Блоха>(testProvider).Where(б => б.МедведьОбитания.Берлога.Cast<Берлога>().Any(бе => бе.Наименование == "Берлога 1" && бе.ЛесРасположения.__PrimaryKey == лес1Pk)).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            // Act.
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, view, new[] { Медведь.Views.МедведьE });

            // Assert.
            Assert.Equal(expected.LimitFunction.ToString(), actual.LimitFunction.ToString());

            // В сравнении Function DetailVariableDef не учитывается, поэтому проверим руками.
            var expectedDvd = (DetailVariableDef)expected.LimitFunction.Parameters[0];
            var actualDvd = (DetailVariableDef)actual.LimitFunction.Parameters[0];

            Assert.True(Equals(expectedDvd.OwnerConnectProp[0], actualDvd.OwnerConnectProp[0]));
        }
    }
}
