namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// This is a test class for LinqToLcsTest and is intended
    /// to contain all LinqToLcsTest Unit Tests.
    /// </summary>
    public class LinqToLcsDetailsTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// Все объекты с детейлами.
        /// </summary>
        [Fact]
        public void GetLcsTestAny()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            // Все кошки с лапами
            new Query<Кошка>(testProvider).Where(o => o.Лапа.Cast<Лапа>().Any()).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef
            {
                ConnectMasterPorp = "Кошка",
                OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey },
                View = Information.GetView("ЛапаE", typeof(Лапа)),
                Type = this.ldef.GetObjectType("Details"),
            };

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = this.ldef.GetFunction("Exist", dvd, true) };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Квантер существования.
        /// </summary>
        [Fact]
        public void GetLcsTestDetailAnyWithLimit()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            // Все кошки с переломами
            new Query<Кошка>(testProvider).Where(o => o.Лапа.Cast<Лапа>().Any(obj => obj.БылиЛиПереломы)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef
            {
                ConnectMasterPorp = "Кошка",
                OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey },
                View = Information.GetView("ЛапаE", typeof(Лапа)),
                Type = this.ldef.GetObjectType("Details"),
            };

            var lf = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.BoolType, "БылиЛиПереломы"));
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = this.ldef.GetFunction("Exist", dvd, lf) };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Квантер общности.
        /// </summary>
        [Fact]
        public void GetLcsTestDetailAllWithLimit()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            // Только те кошки у которых переломаны все лапы
            new Query<Кошка>(testProvider).Where(o => o.Лапа.Cast<Лапа>().All(obj => obj.БылиЛиПереломы)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef
            {
                ConnectMasterPorp = "Кошка",
                OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey },
                View = Information.GetView("ЛапаE", typeof(Лапа)),
                Type = this.ldef.GetObjectType("Details"),
            };

            var lf = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.BoolType, "БылиЛиПереломы"));
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = this.ldef.GetFunction("ExistExact", dvd, lf) };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestDetailAnyConjunction()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(x => x.Лапа.Cast<Лапа>().Any(o => o.Номер == 1 && o.Кошка.Кличка.Contains("кличка"))).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef
            {
                ConnectMasterPorp = "Кошка",
                OwnerConnectProp = new[] { SQLWhereLanguageDef.StormMainObjectKey },
                View = Information.GetView("ЛапаE", typeof(Лапа)),
                Type = this.ldef.GetObjectType("Details"),
            };

            var lf = ldef.GetFunction(
                ldef.funcAND,
                ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.NumericType, "Номер"), 1),
                ldef.GetFunction(ldef.funcLike, new VariableDef(ldef.StringType, "Кошка.Кличка"), "%кличка%"));
            var expected = new LoadingCustomizationStruct(null) { LimitFunction = this.ldef.GetFunction("Exist", dvd, lf) };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestDetailWithCustomOwnerConnectProp()
        {
            var testProvider = new TestLcsQueryProvider<Котенок>();
            new Query<Котенок>(testProvider).Where(
                x => x.Кошка.Лапа.Cast<Лапа>().Any(o => o.ТипЛапы.Название == "передняя")).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef
            {
                ConnectMasterPorp = "Кошка",
                OwnerConnectProp = new[] { "Кошка" },
                View = Information.GetView("ЛапаE", typeof(Лапа)),
                Type = this.ldef.GetObjectType("Details"),
            };

            var lf = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, "ТипЛапы.Название"), "передняя");

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                                       this.ldef.GetFunction("Exist", dvd, lf),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(
                queryExpression, Utils.GetDefaultView(typeof(Котенок)), new[] { Utils.GetDefaultView(typeof(Кошка)) });
            Assert.True(Equals(expected, actual));

            // В сравнении Function DetailVariableDef не учитывается, поэтому проверим руками
            var expectedDvd = (DetailVariableDef)expected.LimitFunction.Parameters[0];
            var actualDvd = (DetailVariableDef)actual.LimitFunction.Parameters[0];

            Assert.True(Equals(expectedDvd.OwnerConnectProp[0], actualDvd.OwnerConnectProp[0]));
        }
    }
}
