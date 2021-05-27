namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// This is a test class for LinqToLcsTest and is intended
    /// to contain all LinqToLcsTest Unit Tests.
    /// </summary>
    public class LinqToLcsMasterTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// A test for GetLcs.
        /// </summary>
        [Fact]
        public void GetLcsTestDataObject()
        {
            var порода = new Порода();
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => порода.Equals(o.Порода)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, порода, new VariableDef(ldef.DataObjectType, "Порода")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterMemberTest()
        {
            var порода = new Порода
            {
                Название = "тест",
            };

            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.Название.Equals(порода.Название)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(
                            ldef.funcEQ, new VariableDef(ldef.StringType, "Порода.Название"), порода.Название),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterStringMemberTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.Название.Contains("ки")).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcLike, new VariableDef(ldef.StringType, "Порода.Название"), "%ки%"),
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterOrTest()
        {
            var порода1 = new Порода();
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода == порода1 || o.Порода.Название.Equals("тест1")).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcOR,
                    ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.DataObjectType, "Порода"), порода1),
                    ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, "Порода.Название"), "тест1")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMastersOrTest()
        {
            var порода1 = new Порода();
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода == порода1 || o.Порода == порода1).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(
                            ldef.funcOR,
                            ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.DataObjectType, "Порода"), порода1),
                            ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.DataObjectType, "Порода"), порода1)),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterNullTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода == null).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                   ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.DataObjectType, "Порода")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterNotNullTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода != null).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                   ldef.GetFunction(ldef.funcNotIsNull, new VariableDef(ldef.DataObjectType, "Порода")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterNotNullTestRevert()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => null != o.Порода).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                   ldef.GetFunction(ldef.funcNotIsNull, new VariableDef(ldef.DataObjectType, "Порода")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterNullTestRevert()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => null == o.Порода).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                   ldef.GetFunction(ldef.funcIsNull, new VariableDef(ldef.DataObjectType, "Порода")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterNullAndTest()
        {
            var порода1 = new Порода();
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => (o.Порода != null) && (o.Порода != порода1)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(
                        ldef.funcAND,
                        ldef.GetFunction(ldef.funcNotIsNull, new VariableDef(ldef.DataObjectType, "Порода")),
                        ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.DataObjectType, "Порода"), порода1)),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsManyMastersPropertysTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.Название == o.Порода.Название).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.DataObjectType, "Порода.Название"), new VariableDef(ldef.DataObjectType, "Порода.Название")),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsNullIsNullTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => null == null).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(ldef.paramTrue),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка правильности формирования lcs, если в linq-выражении используется мастер второго уровня.
        /// </summary>
        [Fact]
        public void GetLcsSecondLevelTest()
        {
            var breedType = new ТипПороды() { Название = "Чеширский Улыбчивый" };
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.ТипПороды == breedType).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(
                            ldef.funcEQ,
                            new VariableDef(ldef.DataObjectType, Information.ExtractPropertyPath<Кошка>(x => x.Порода.ТипПороды)),
                            breedType),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMemberOfSecondLevelTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.ТипПороды.Название == "Блатная порода").ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.StringType, "Порода.ТипПороды.Название"), "Блатная порода"),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMethodOfMemberOfSecondLevelTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.ТипПороды.Название.StartsWith("Блат")).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(ldef.funcLike, new VariableDef(ldef.StringType, "Порода.ТипПороды.Название"), "Блат%"),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsDateTimeMemberOfSecondLevelTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            DateTime date = DateTime.Today;

            new Query<Кошка>(testProvider).Where(o => o.Порода.ТипПороды.ДатаРегистрации.Date <= DateTime.Today).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                        ldef.GetFunction(
                            ldef.funcLEQ,
                            ldef.GetFunction(
                                ldef.funcOnlyDate,
                                new VariableDef(ldef.DateTimeType, "Порода.ТипПороды.ДатаРегистрации")),
                            ldef.GetFunction(ldef.funcOnlyDate, ldef.GetFunction("TODAY"))),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterParentPKTest()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            var strGuid = Guid.NewGuid().ToString();
            new Query<Кошка>(testProvider).FirstOrDefault(o => (string)(o.Порода.__PrimaryKey) == strGuid);
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.GuidType, "Порода"), strGuid),
            };
            expected.ReturnType = LcsReturnType.Object;

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsMasterParentPKTestToString()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            var strGuid = Guid.NewGuid().ToString();
            new Query<Кошка>(testProvider).FirstOrDefault(o => o.Порода.__PrimaryKey.ToString() == strGuid);
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                                       ldef.GetFunction(
                                           ldef.funcEQ,
                                           new VariableDef(ldef.GuidType, "Порода"),
                                           strGuid),
                ReturnType = LcsReturnType.Object,
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест сравнения мастера через "мастер.__PrimaryKey.Equals(мастер2)".
        /// </summary>
        [Fact]
        public void GetLcsMasterEqualsMaster()
        {
            var guid = Guid.NewGuid();
            var порода1 = new Порода() { __PrimaryKey = guid };
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.__PrimaryKey.Equals(guid)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.GuidType, "Порода"), guid),
            };

            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест сравнения мастера через "мастер.__PrimaryKey.Equals(null)".
        /// </summary>
        [Fact]
        public void GetLcsMasterEqualsNull()
        {
            var guid = Guid.NewGuid();
            var порода1 = new Порода() { __PrimaryKey = guid };
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Порода.__PrimaryKey.Equals(null)).ToArray();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    ldef.GetFunction(ldef.funcEQ, new VariableDef(ldef.GuidType, "Порода"), null),
            };

            Assert.True(Equals(expected, actual));
        }
    }
}
