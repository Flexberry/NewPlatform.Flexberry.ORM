namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.Exceptions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Класс для тестирования построения запросов при наличии псевдодетейлов
    /// (то есть когда классы связаны ассоциацией).
    /// </summary>
    public class LinqToLcsWithPseudoDetailsTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        #region Простейшие ограничения, ограничения с Any

        /// <summary>
        /// Все объекты с псевдодетейлами (проверяется без наложения дополнительного условия).
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestSimpleAny()
        {
            var testProvider = new TestLcsQueryProvider<Порода>();

            // Все породы, для которых определены кошки
            new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                x => x.Any()).ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Кошка__Порода",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = this.ldef.GetFunction("Exist", dvd, ldef.GetFunction(ldef.paramTrue)),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));

            // Совершенно аналогично должно работать, если вместо Information.ExtractPropertyPath<Кошка>(x => x.Порода) передать просто x => x.Порода
            new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                x => x.Порода,
                x => x.Any()).ToList();
            Expression queryExpression2 = testProvider.InnerExpression;
            LoadingCustomizationStruct actual2 = LinqToLcs.GetLcs(queryExpression2, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual2));
        }

        /// <summary>
        /// Проверка условий, которым должно отвечать ограничение.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestTryConstLimit()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Порода>();

                // Ограничение не должно быть выполнено в виде константы
                new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                x => true).ToList();
                Expression queryExpression = testProvider.InnerExpression;
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
                Assert.True(false, "Выполнилось ограничение не в виде подзапроса");
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Проверка условий, которым должно отвечать ограничение.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestTryNotSubqueryLimit()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Порода>();

                // Ограничение не должно быть выполнено в виде нескольких подзапросов
                new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                y => y.Any(x => x.Кличка != "Барсик") && y.Any(x => x.Кличка != "Мурзик")).ToList();
                Expression queryExpression = testProvider.InnerExpression;
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
                Assert.True(false, "Выполнилось ограничение не в виде подзапроса");
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Проверка условий, которым должно отвечать ограничение.
        /// </summary>
        [Fact(Skip = "вернуть выполнение этого теста.")]
        public void GetLcsByPseudoDetailLimitTestTryNotSubSubqueryLimit()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Порода>();

                // Ограничение не должно содержать вложенные подзапросы
                new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                y => y.Any(x => x.Кличка != "Барсик" && y.Any())).ToList();
                Expression queryExpression = testProvider.InnerExpression;
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
                Assert.True(false, "Выполнилось ограничение не в виде подзапроса");
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Все объекты, где хотя бы один псевдодетейл удовлетворяет условию.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestCondition()
        {
            var testProvider = new TestLcsQueryProvider<Порода>();

            // Все породы, для которых существуют кошки, у которых кличка не "Барсик" и не "Мурзик"
            new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                y => y.Any(x => x.Кличка != "Барсик" && x.Кличка != "Мурзик")).ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Кошка__Порода",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var lf = ldef.GetFunction(
                ldef.funcAND,
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Барсик"),
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Мурзик"));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = this.ldef.GetFunction("Exist", dvd, lf),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));

            // Совершенно аналогично должно работать, если вместо Information.ExtractPropertyPath<Кошка>(x => x.Порода) передать просто x => x.Порода
            new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                x => x.Порода,
                y => y.Any(x => x.Кличка != "Барсик" && x.Кличка != "Мурзик")).ToList();
            Expression queryExpression2 = testProvider.InnerExpression;
            LoadingCustomizationStruct actual2 = LinqToLcs.GetLcs(queryExpression2, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual2));
        }

        /// <summary>
        /// Проверяем, что для настоящих детейлов ограничение по типу псевдодетейла корректно отработает.
        /// </summary>
        [Fact]
        public void GetLcsByDetailLimitTestSimpleAny()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            // Все кошки с лапами
            new Query<Кошка>(testProvider).Where<Кошка, Лапа>(
                Information.GetView("ЛапаE", typeof(Лапа)),
                Information.ExtractPropertyPath<Лапа>(x => x.Кошка),
                x => x.Any()).ToArray();

            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Лапа__Кошка",
                Information.GetView("ЛапаE", typeof(Лапа)),
                "Кошка",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = this.ldef.GetFunction("Exist", dvd, ldef.GetFunction(ldef.paramTrue)) };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));

            // Совершенно аналогично должно работать, если вместо Information.ExtractPropertyPath<Кошка>(x => x.Порода) передать просто x => x.Порода
            new Query<Кошка>(testProvider).Where<Кошка, Лапа>(
                Information.GetView("ЛапаE", typeof(Лапа)),
                x => x.Кошка,
                x => x.Any()).ToArray();
            Expression queryExpression2 = testProvider.InnerExpression;
            LoadingCustomizationStruct actual2 = LinqToLcs.GetLcs(queryExpression2, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual2));
        }

        /// <summary>
        /// Тестирование того, что проводится проверка передаваемых параметров.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestWrongParameters()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Кошка>();

                // Передаём View от неправильного класса
                new Query<Порода>(testProvider).Where<Порода, Кошка>(
                    Information.GetView("ЛапаE", typeof(Лапа)),
                    Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                    x => x.Any()).ToList();
                Expression queryExpression = testProvider.InnerExpression;
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
                Assert.True(false, "Выполнено выражение с некорректным представлением для псевдодетейла.");
            });
            Assert.IsType(typeof(CantFindViewException), exception);
        }

        /// <summary>
        /// Проверяем, что для настоящих детейлов ограничение по типу псевдодетейла корректно отработает, причём указывать агрегирующее свойство не будем.
        /// </summary>
        [Fact]
        public void GetLcsByDetailLimitTestSpecialVariant()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            // Все кошки с лапами
            new Query<Кошка>(testProvider).Where<Кошка, Лапа>(
                Information.GetView("ЛапаE", typeof(Лапа)),
                x => x.Any()).ToArray();

            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Лапа__Кошка",
                Information.GetView("ЛапаE", typeof(Лапа)),
                "Кошка",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var expected = new LoadingCustomizationStruct(null) { LimitFunction = this.ldef.GetFunction("Exist", dvd, ldef.GetFunction(ldef.paramTrue)) };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем, что для псевдодетейлов ограничение по упрощённой схеме не отработает, поскольку не найдётся свойство-агрегатор.
        /// </summary>
        [Fact]
        public void GetLcsByDetailLimitTestWrongDetail()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Кошка>();
                new Query<Порода>(testProvider).Where<Порода, Кошка>(
                        Information.GetView("КошкаE", typeof(Кошка)),
                        y => y.Any()).ToList();
                Expression queryExpression = testProvider.InnerExpression;
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
                Assert.True(false, "Создано ограничение по упрощённой схеме для ненастоящего детейла.");
            });
            Assert.IsType(typeof(NotFoundAggregatorProperty), exception);
        }

        #endregion Простейшие ограничения, ограничения с Any

        #region Oграничения с All

        /// <summary>
        /// Все объекты, где все псевдодетейлы удовлетворяют условию.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestConditionAll()
        {
            var testProvider = new TestLcsQueryProvider<Порода>();

            // Все породы, в которые входят только кошки, не носящие клички ни "Барсик", ни "Мурзик"
            new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                y => y.All(x => x.Кличка != "Барсик" && x.Кличка != "Мурзик")).ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Кошка__Порода",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var lf = ldef.GetFunction(
                ldef.funcAND,
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Барсик"),
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Мурзик"));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = this.ldef.GetFunction(ldef.funcExistExact, dvd, lf),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем неподдерживаемую ситуацию вида  y => y.All(x => y.Any())).
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestWrongAll()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var testProvider = new TestLcsQueryProvider<Порода>();

                new Query<Порода>(testProvider).Where<Порода, Кошка>(
                       Information.GetView("КошкаE", typeof(Кошка)),
                       Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                       y => y.All(x => x.Кличка != "Барсик" && y.Any())).ToList();

                Expression queryExpression = testProvider.InnerExpression;

                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
                Assert.True(false, "Было выполнено неподдерживаемое выражение");
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        #endregion Oграничения с All

        #region Проверка правильного копирования ConnectMasterProperty

        /// <summary>
        /// Все объекты с псевдодетейлами (проверяется без наложения дополнительного условия).
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestSimpleAnyWithCmp()
        {
            var testProvider = new TestLcsQueryProvider<Порода>();

            // Все породы, для которых определены кошки
            new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                x => x.Any(),
                "SomeCmp").ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "SomeCmp",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = this.ldef.GetFunction("Exist", dvd, ldef.GetFunction(ldef.paramTrue)),
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Все объекты, где все псевдодетейлы удовлетворяют условию.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestConditionAllWithCmp()
        {
            var testProvider = new TestLcsQueryProvider<Порода>();

            // Все породы, в которые входят только кошки, не носящие клички ни "Барсик", ни "Мурзик"
            new Query<Порода>(testProvider).Where<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                y => y.All(x => x.Кличка != "Барсик" && x.Кличка != "Мурзик"),
                "SomeCmp").ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "SomeCmp",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var lf = ldef.GetFunction(
                ldef.funcAND,
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Барсик"),
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Мурзик"));

            var expected = new LoadingCustomizationStruct(null)
            { LimitFunction = this.ldef.GetFunction(ldef.funcExistExact, dvd, lf) };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));
        }

        #endregion Проверка правильного копирования ConnectMasterProperty

        #region Поддержка функциональности по работе с псевдодетейлами с классом PseudoDetail

        /// <summary>
        /// Все объекты с псевдодетейлами.
        /// Объект типа PseudoDetail создаётся отдельно от Linq-выражения.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestSimpleAny2()
        {
            var pseudoDetail = new PseudoDetail<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода));

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Кошка__Порода",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            // Все породы, для которых есть кошки, кличка которых не Мурзик.
            CompareLcsWithLimitCommon(
                y => pseudoDetail.Any(x => x.Кличка != "Мурзик"),
                this.ldef.GetFunction("Exist", dvd, ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Мурзик")));
        }

        /// <summary>
        /// Все объекты с псевдодетейлами.
        /// Объект типа PseudoDetail создаётся непосредственно в Linq-выражении.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestSimpleAny3()
        {
            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Кошка__Порода",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            // Все породы, для которых есть кошки, кличка которых не Мурзик.
            CompareLcsWithLimitCommon(
                y => new PseudoDetail<Порода, Кошка>(
                        Information.GetView("КошкаE", typeof(Кошка)),
                        Information.ExtractPropertyPath<Кошка>(x => x.Порода))
                        .Any(x => x.Кличка != "Мурзик"),
                this.ldef.GetFunction("Exist", dvd, ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Мурзик")));
        }

        /// <summary>
        /// Все объекты, где все псевдодетейлы удовлетворяют условию.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestConditionAll2()
        {
            var pseudoDetail = new PseudoDetail<Порода, Кошка>(
                Information.GetView("КошкаE", typeof(Кошка)),
                Information.ExtractPropertyPath<Кошка>(x => x.Порода));

            var dvd = new DetailVariableDef(
                this.ldef.GetObjectType("Details"),
                "Кошка__Порода",
                Information.GetView("КошкаE", typeof(Кошка)),
                "Порода",
                new[] { SQLWhereLanguageDef.StormMainObjectKey });

            var lf = ldef.GetFunction(
                ldef.funcAND,
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Барсик"),
                ldef.GetFunction(ldef.funcNEQ, new VariableDef(ldef.StringType, "Кличка"), "Мурзик"));

            // Все породы, в которые входят только кошки, не носящие клички ни "Барсик", ни "Мурзик".
            CompareLcsWithLimitCommon(y => pseudoDetail.All(x => x.Кличка != "Барсик" && x.Кличка != "Мурзик"), this.ldef.GetFunction(ldef.funcExistExact, dvd, lf));
        }

        #region Проверяем разные конструкторы для PseudoDetail.

        /// <summary>
        /// Все объекты с псевдодетейлами (дополнительное условие не накладывается).
        /// Объект типа PseudoDetail создаётся отдельно от Linq-выражения.
        /// Проверяем разные конструкторы для PseudoDetail.
        /// </summary>
        [Fact]
        public void GetLcsByPseudoDetailLimitTestSimpleAny4()
        { // Все породы, для которых определены кошки.
            ComparePseudoDetailWithDetailVariableDef(
                new PseudoDetail<Порода, Кошка>(
                                            Information.GetView("КошкаE", typeof(Кошка)),
                                            Information.ExtractPropertyPath<Кошка>(x => x.Порода)),
                new DetailVariableDef(
                    this.ldef.GetObjectType("Details"),
                    "Кошка__Порода",
                    Information.GetView("КошкаE", typeof(Кошка)),
                    "Порода",
                    new[] { SQLWhereLanguageDef.StormMainObjectKey }));
        }

        /// <summary>
        /// Проверяем разные конструкторы для PseudoDetail.
        /// </summary>
        [Fact]
        public void CheckPseudoDetailConstructor1()
        { // Все породы, для которых определены кошки.
            ComparePseudoDetailWithDetailVariableDef(
                new PseudoDetail<Порода, Кошка>(
                                            Information.GetView("КошкаE", typeof(Кошка)),
                                            x => x.Порода),
                new DetailVariableDef(
                    this.ldef.GetObjectType("Details"),
                    "Кошка__Порода",
                    Information.GetView("КошкаE", typeof(Кошка)),
                    "Порода",
                    new[] { SQLWhereLanguageDef.StormMainObjectKey }));
        }

        /// <summary>
        /// Проверяем разные конструкторы для PseudoDetail.
        /// </summary>
        [Fact]
        public void CheckPseudoDetailConstructor2()
        { // Все породы, для которых определены кошки.
            const string masterToDetailPropertyName = "SomePropertyName";

            ComparePseudoDetailWithDetailVariableDef(
                new PseudoDetail<Порода, Кошка>(
                                            Information.GetView("КошкаE", typeof(Кошка)),
                                            x => x.Порода,
                                            masterToDetailPropertyName),
                new DetailVariableDef(
                    this.ldef.GetObjectType("Details"),
                    masterToDetailPropertyName,
                    Information.GetView("КошкаE", typeof(Кошка)),
                    "Порода",
                    new[] { SQLWhereLanguageDef.StormMainObjectKey }));
        }

        /// <summary>
        /// Проверяем разные конструкторы для PseudoDetail.
        /// </summary>
        [Fact]
        public void CheckPseudoDetailConstructor3()
        { // Все породы, для которых определены кошки.
            const string masterToDetailPropertyName = "SomePropertyName";
            var masterConnectProperties = new string[] { "Property1", "Property2" };

            ComparePseudoDetailWithDetailVariableDef(
                new PseudoDetail<Порода, Кошка>(
                                            Information.GetView("КошкаE", typeof(Кошка)),
                                            x => x.Порода,
                                            masterToDetailPropertyName,
                                            masterConnectProperties),
                new DetailVariableDef(
                    this.ldef.GetObjectType("Details"),
                    masterToDetailPropertyName,
                    Information.GetView("КошкаE", typeof(Кошка)),
                    "Порода",
                    masterConnectProperties));
        }

        /// <summary>
        /// Проверяем разные конструкторы для PseudoDetail.
        /// </summary>
        [Fact]
        public void CheckPseudoDetailConstructor4()
        { // Все породы, для которых определены кошки.
            const string masterToDetailPropertyName = "SomePropertyName";
            var masterConnectProperties = new string[] { "Property1", "Property2" };

            ComparePseudoDetailWithDetailVariableDef(
                new PseudoDetail<Порода, Кошка>(
                                            Information.GetView("КошкаE", typeof(Кошка)),
                                            Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                                            masterToDetailPropertyName,
                                            masterConnectProperties),
                new DetailVariableDef(
                    this.ldef.GetObjectType("Details"),
                    masterToDetailPropertyName,
                    Information.GetView("КошкаE", typeof(Кошка)),
                    "Порода",
                    masterConnectProperties));
        }

        /// <summary>
        /// Проверяем разные конструкторы для PseudoDetail.
        /// </summary>
        [Fact]
        public void CheckPseudoDetailConstructor5()
        { // Все породы, для которых определены кошки.
            const string masterToDetailPropertyName = "SomePropertyName";

            ComparePseudoDetailWithDetailVariableDef(
                new PseudoDetail<Порода, Кошка>(
                                            Information.GetView("КошкаE", typeof(Кошка)),
                                            Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                                            masterToDetailPropertyName,
                                            null),
                new DetailVariableDef(
                    this.ldef.GetObjectType("Details"),
                    masterToDetailPropertyName,
                    Information.GetView("КошкаE", typeof(Кошка)),
                    "Порода",
                    new[] { SQLWhereLanguageDef.StormMainObjectKey }));
        }

        /// <summary>
        /// Проверяем разные конструкторы для PseudoDetail.
        /// </summary>
        [Fact]
        public void CheckPseudoDetailConstructor6()
        { // Все породы, для которых определены кошки.
            const string masterToDetailPropertyName = "SomePropertyName";

            ComparePseudoDetailWithDetailVariableDef(
                new PseudoDetail<Порода, Кошка>(
                                            Information.GetView("КошкаE", typeof(Кошка)),
                                            Information.ExtractPropertyPath<Кошка>(x => x.Порода),
                                            masterToDetailPropertyName,
                                            new string[] { }),
                new DetailVariableDef(
                    this.ldef.GetObjectType("Details"),
                    masterToDetailPropertyName,
                    Information.GetView("КошкаE", typeof(Кошка)),
                    "Порода",
                    new[] { SQLWhereLanguageDef.StormMainObjectKey }));
        }

        /// <summary>
        /// Проверяем генерацию <see cref="DetailVariableDef"/> из PseudoDetail.
        /// </summary>
        /// <param name="pseudoDetail">Псевдодетейл из linq.</param>
        /// <param name="detailVariableDef">Детейл из lcs.</param>
        private void ComparePseudoDetailWithDetailVariableDef(PseudoDetail<Порода, Кошка> pseudoDetail, DetailVariableDef detailVariableDef)
        {
            CompareLcsWithLimitCommon(y => pseudoDetail.Any(), this.ldef.GetFunction(ldef.funcExist, detailVariableDef, true));
        }

        #endregion Проверяем разные конструкторы для PseudoDetail.

        /// <summary>
        /// Проверка соответствия linq сгенерированному lcs.
        /// </summary>
        /// <param name="predicate">Предикат в linq.</param>
        /// <param name="limitFunction">Функция ограничения в терминах lcs.</param>
        private void CompareLcsWithLimitCommon(Expression<Func<Порода, bool>> predicate, Function limitFunction)
        {
            var testProvider = new TestLcsQueryProvider<Порода>();
            new Query<Порода>(testProvider).Where(predicate).ToList();

            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = limitFunction,
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Порода)));
            Assert.True(Equals(expected, actual));
        }

        #endregion Поддержка функциональности по работе с псевдодетейлами с классом PseudoDetail
    }
}
