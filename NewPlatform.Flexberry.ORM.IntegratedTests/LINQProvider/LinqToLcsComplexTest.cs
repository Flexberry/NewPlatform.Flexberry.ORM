namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// This is a test class for LinqToLcsTest and is intended
    /// to contain all LinqToLcsTest Unit Tests
    /// </summary>
    
    public class LinqToLcsComplexTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;

        /// <summary>
        /// A test for GetLcs
        /// </summary>
        [Fact]
        public void GetLcsTestPrimaryKey()
        {
            const string Guid = "6211E0DE-3E7A-4A68-866A-AB206A005B1C";
            IQueryable<DataObject> queryList = new List<DataObject>().AsQueryable();
            IQueryable<DataObject> query = from pn in queryList where pn.__PrimaryKey == Guid select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        this.ldef.GetFunction(
                            this.ldef.funcEQ, new VariableDef(this.ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), Guid)
                };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest2()
        {
            const int Number = 0;
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер > Number select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        this.ldef.GetFunction(this.ldef.funcG, new VariableDef(this.ldef.NumericType, "Номер"), Number)
                };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest3()
        {
            const int Age = 0;
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер < Age select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        this.ldef.GetFunction(this.ldef.funcL, new VariableDef(this.ldef.NumericType, "Номер"), Age)
                };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest4()
        {
            const int Age = 0;
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер >= Age select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        this.ldef.GetFunction(this.ldef.funcGEQ, new VariableDef(this.ldef.NumericType, "Номер"), Age)
                };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest5()
        {
            const int Age = 0;
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер <= Age select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        this.ldef.GetFunction(this.ldef.funcLEQ, new VariableDef(this.ldef.NumericType, "Номер"), Age)
                };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest7()
        {
            const string Pattern = "ош";
            const string ldefPattern = "%ош%";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList
                                                        where pn.Кличка.Contains(Pattern) && pn.Кличка != "кошка"
                                                        select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        ldef.GetFunction(
                            ldef.funcAND,
                            ldef.GetFunction(
                                this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
                            ldef.GetFunction(this.ldef.funcNEQ, new VariableDef(this.ldef.StringType, "Кличка"), "кошка"))
                };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest8()
        {
            const string Pattern = "ош";
            const string ldefPattern = "%ош%";
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList
                                                        where pn.Кличка.Contains(Pattern) || pn.Кличка != "кошка"
                                                        select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        ldef.GetFunction(
                            ldef.funcOR,
                            ldef.GetFunction(
                                this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), ldefPattern),
                            ldef.GetFunction(this.ldef.funcNEQ, new VariableDef(this.ldef.StringType, "Кличка"), "кошка"))
                };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest9()
        {
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = (from pn in queryList select pn).Take(1);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null) { ReturnTop = 1 };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTest10()
        {
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IOrderedQueryable<Кошка> query = from pn in queryList
                                                        orderby pn.Кличка ascending , pn.Порода descending
                                                        select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
                {
                    ColumnsSort =
                        new[]
                            {
                                new ColumnsSortDef("Кличка", SortOrder.Asc), new ColumnsSortDef("Порода", SortOrder.Desc)
                            }
                };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestSortMasterField()
        {
            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IOrderedQueryable<Кошка> query = from pn in queryList
                                             orderby pn.Порода.Название ascending
                                             select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                ColumnsSort =
                        new[]
                            {
                                new ColumnsSortDef(Information.ExtractPropertyPath<Кошка>(c => c.Порода.Название), SortOrder.Asc)
                            }
            };
            
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Кошка.Views.КошкаE);
            Assert.True(Equals(expected, actual));

            IQueryable<Кошка> query2 = (from pn in queryList select pn).OrderBy(pn => pn.Порода == null ? null : pn.Порода.Название);
            Expression queryExpression2 = query2.Expression;
            LoadingCustomizationStruct actual2 = LinqToLcs.GetLcs(queryExpression2, Кошка.Views.КошкаE);
            Assert.True(Equals(expected, actual2));
        }



        [Fact]
        public void GetLcsTestAny()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            Queryable.Any(new Query<Кошка>(testProvider), o => o.Кличка.Contains("ош"));
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        this.ldef.GetFunction(
                            this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), "%ош%"),
                    ReturnType = LcsReturnType.Any
                };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestCount()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            Queryable.Count(new Query<Кошка>(testProvider), o => o.Кличка.Contains("ош"));
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcLike, new VariableDef(this.ldef.StringType, "Кличка"), "%ош%"),
                ReturnType = LcsReturnType.Count
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestSelf()
        {
            var кошка = new Кошка();

            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where pn == кошка select pn;
            Expression queryExpression = query.Expression;

            var expected = new LoadingCustomizationStruct(null)
                {
                    LimitFunction =
                        this.ldef.GetFunction(
                            this.ldef.funcEQ, new VariableDef(this.ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey), кошка.__PrimaryKey)
                };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestSelfInvert()
        {
            var кошка = new Кошка();

            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where кошка == pn select pn;
            Expression queryExpression = query.Expression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ, кошка.__PrimaryKey, new VariableDef(this.ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey))
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для преобразования вызова метода Contains у 
        /// массива в функцию IN
        /// </summary>
        [Fact]
        public void GetLcsTestInNewArrayInitialization()
        {
            const string pattern = "Кошка";

            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where (new[] { pattern, pattern }).Contains(pn.Кличка) select pn;
            Expression queryExpression = query.Expression;

            var expected = new LoadingCustomizationStruct(null)
                               {
                                   LimitFunction =
                                       ldef.GetFunction(
                                           ldef.funcIN,
                                           new VariableDef(ldef.StringType, "Кличка"),
                                           pattern,
                                           pattern)
                               };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestInList()
        {
            var list = new List<string> { "Кошка", "Кошка" };

            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(pn => list.Contains(pn.Кличка));
            Expression queryExpression = query.Expression;

            var queryCollection = new List<object> { new VariableDef(ldef.StringType, "Кличка") };
            queryCollection.AddRange(list);

            var expected = new LoadingCustomizationStruct(null)
                               {
                                   LimitFunction = ldef.GetFunction(ldef.funcIN, queryCollection.ToArray())
                               };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestMasterInList()
        {
            var list = new List<string> { "{pk1}", "{pk2}" };

            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(pn => list.Contains(pn.Порода.__PrimaryKey.ToString()));
            Expression queryExpression = query.Expression;

            var queryCollection = new List<object> { new VariableDef(ldef.GuidType, "Порода") };
            queryCollection.AddRange(list);

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcIN, queryCollection.ToArray())
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestMasterSecondLevelPropertyInList()
        {
            var list = new List<string> { "ТипПороды1", "ТипПороды2" };

            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = queryList.Where(pn => list.Contains(pn.Порода.ТипПороды.Название));
            Expression queryExpression = query.Expression;

            var queryCollection = new List<object> { new VariableDef(ldef.StringType, "Порода.ТипПороды.Название") };
            queryCollection.AddRange(list);

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcIN, queryCollection.ToArray())
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestInListWithMethodCall()
        {
            var list = new List<string> { "гуид1", "гуид2" };

            IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
            IQueryable<Кошка> query = from pn in queryList where list.Contains(pn.__PrimaryKey.ToString()) select pn;
            Expression queryExpression = query.Expression;

            var queryCollection = new List<object> { new VariableDef(ldef.GuidType, SQLWhereLanguageDef.StormMainObjectKey) };
            queryCollection.AddRange(list);

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcIN, queryCollection.ToArray())
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест для выражения “строка-константа”.StartsWith([выражение])
        /// (должно бросаться исключение)
        /// </summary>
        [Fact]
        public void GetLcsStartsWithExpr()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                var pattern = "Кошка";

                IQueryable<Кошка> queryList = new List<Кошка>().AsQueryable();
                IQueryable<Кошка> query =
                    from pn in queryList
                    where "kokoko".StartsWith(pn.Кличка)
                    select pn;
                Expression queryExpression = query.Expression;
                LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));
            });
            Assert.IsType(typeof(NotSupportedException), exception);
        }

        /// <summary>
        /// Тест для проверки выполнения простейших операций с переменными при работе в LinqProvider.
        /// </summary>
        [Fact]
        public void TestQueryCompilation()
        {
            Assert.Equal("prefixpostfix", UtilsLcs.TryExecuteBinaryOberation(ExpressionType.Add, "postfix", "prefix"));
            Assert.Equal(null, UtilsLcs.TryExecuteBinaryOberation(ExpressionType.Multiply, "postfix", "prefix"));
            Assert.Equal(3, UtilsLcs.TryExecuteBinaryOberation(ExpressionType.Add, 1, 2));
            Assert.Equal(1, UtilsLcs.TryExecuteBinaryOberation(ExpressionType.Subtract, 1, 2));
            Assert.Equal(2, UtilsLcs.TryExecuteBinaryOberation(ExpressionType.Multiply, 1, 2));
            Assert.Equal(2, UtilsLcs.TryExecuteBinaryOberation(ExpressionType.Divide, 1, 2));
            Assert.Equal(null, UtilsLcs.TryExecuteBinaryOberation(ExpressionType.Add,
                new VariableDef(ldef.StringType, "OperationType"), "123"));
        }

        /// <summary>
        /// Тест для проверки выполнения простейших операций с переменными при работе в LinqProvider.
        /// </summary>
        [Fact]
        public void TestQueryCompilation2()
        {
            string prefix = "prefix";
            string postfix = "postfix";
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(x => x.Кличка == prefix + postfix).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Кошка)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(
                                        ldef.funcEQ,
                                        new VariableDef(this.ldef.StringType, "Кличка"),
                                        prefix + postfix)
            };

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        /// <summary>
        /// Тест для проверки выполнения простейших операций с переменными при работе в LinqProvider.
        /// </summary>
        [Fact]
        public void TestQueryCompilation3()
        {
            int prefix = 1;
            int postfix = 2;
            int other = 3;
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.Размер == prefix + postfix + other).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(
                                        ldef.funcEQ,
                                        new VariableDef(this.ldef.NumericType, "Размер"),
                                        6)
            };

            Assert.Equal(expected.ToString(), actual.ToString());
        }
    }
}
