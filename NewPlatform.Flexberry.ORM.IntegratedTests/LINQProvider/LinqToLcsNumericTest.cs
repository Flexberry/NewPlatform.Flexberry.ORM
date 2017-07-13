namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Windows.Forms;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;

    
    public class LinqToLcsNumericTest
    {
        private readonly ExternalLangDef ldef = ExternalLangDef.LanguageDef;
        private const int Number1 = 2;
        private const int Number2 = 8;
        
        /// <summary>
        /// Тест арифметических операций
        /// </summary>
        [Fact]
        public void GetLcsTestSumm()
        {
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер == Number1 + Number2 + pn.Размер select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.NumericType, "Номер"),
                        ldef.GetFunction(
                            ldef.funcPlus,
                            new VariableDef(this.ldef.NumericType, "Размер"),
                            10))
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тест Equals
        /// </summary>
        [Fact]
        public void GetLcsTestIntEquals()
        {
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер.Equals(Number2) select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.NumericType, "Номер"),
                        Number2)
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }
        /// <summary>
        /// Тест Equals наоборот
        /// </summary>
        [Fact]
        public void GetLcsTestIntEqualsReverse()
        {
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = (from pn in queryList where Number2.Equals(pn.Номер) select pn);
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        Number2,
                        new VariableDef(this.ldef.NumericType, "Номер"))
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }


        [Fact]
        public void GetLcsTestSub()
        {
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер == Number2 - pn.Размер select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.NumericType, "Номер"),
                        ldef.GetFunction(
                            ldef.funcMinus,
                            new VariableDef(this.ldef.NumericType, "Размер"),
                            Number2))
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestDiv()
        {
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер == Number2 / pn.Размер select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.NumericType, "Номер"),
                        ldef.GetFunction(
                            ldef.funcDiv,
                            new VariableDef(this.ldef.NumericType, "Размер"),
                            Number2))
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestMulti()
        {
            IQueryable<Лапа> queryList = new List<Лапа>().AsQueryable();
            IQueryable<Лапа> query = from pn in queryList where pn.Номер == Number2 * pn.Размер select pn;
            Expression queryExpression = query.Expression;
            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.NumericType, "Номер"),
                        ldef.GetFunction(
                            ldef.funcSub,
                            new VariableDef(this.ldef.NumericType, "Размер"),
                            Number2))
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestIntParse()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            var predicate = (Expression<Func<Лапа, bool>>)(o => o.Номер == int.Parse("1"));
            new Query<Лапа>(testProvider).Where(predicate).ToArray();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcEQ,
                        new VariableDef(this.ldef.NumericType, "Номер"),
                        int.Parse("1"))
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestDouble()
        {
            const double Size = 43.23423;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерDouble < Size).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcL,
                        new VariableDef(this.ldef.NumericType, "РазмерDouble"),
                        Size)
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверка работы с типом float.
        /// </summary>
        [Fact]
        public void GetLcsTestFloat()
        {
            const float Size = 43.76F;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерFloat < Size).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcL,
                        new VariableDef(this.ldef.NumericType, "РазмерFloat"),
                        Size)
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тип int? должен вести себя в LanguageDef как обычный int
        /// </summary>
        [Fact]
        public void GetLcsTestNullableInt()
        {
            int? size = 4;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt < size).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcL,
                        new VariableDef(this.ldef.NumericType, "РазмерNullableInt"),
                        size)
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тип int? должен уметь сравниваться с null.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithNull()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt == null).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcIsNull,
                        new VariableDef(this.ldef.NumericType, "РазмерNullableInt"))
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тип int? должен уметь сравниваться с null.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithNotNull()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt != null).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcNotIsNull,
                        new VariableDef(this.ldef.NumericType, "РазмерNullableInt"))
            };

            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        [Fact]
        public void GetLcsTestDecimal()
        {
            const decimal Size = (decimal)43.23423;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерDecimal < Size).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcL,
                        new VariableDef(this.ldef.NumericType, "РазмерDecimal"),
                        Size)
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тип int? должен вести себя в LanguageDef как обычный int
        /// </summary>
        [Fact]
        public void GetLcsTestNullableDecimal()
        {
            int? size = 4;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableDecimal < size).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcL,
                        new VariableDef(this.ldef.NumericType, "РазмерNullableDecimal"),
                        size)
            };
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тип int? должен уметь сравниваться с null.
        /// Сравниваем с int?, который имеет значение null.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithNull2()
        {
            // Данный код просто показывает, что классическое linq возвращает, сравнивая int с null, при этом не падая.
            var someList = new List<int?>() {1, 2, 3, null};
            int? nullableInt = null;
            var result = someList.Where(x => x > nullableInt).ToList();

            Assert.Equal(0, result.Count);

            // Далее наш linqprovider.
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt > nullableInt).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcNOT, this.ldef.GetFunction(this.ldef.paramTrue))
            };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Тип int? должен уметь сравниваться с null.
        /// Выполняется сравнение с null напрямую.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithNull3()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt > null).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction = ldef.GetFunction(ldef.funcNOT, this.ldef.GetFunction(this.ldef.paramTrue))
            };
            Assert.True(Equals(expected, actual));
        }

        /// <summary>
        /// Проверяем, что от Nullable-типа в LinqProvider можно взять Value.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithValue()
        {
            int size = 4;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt.Value < size).ToList();
            Expression queryExpression = testProvider.InnerExpression;
            LoadingCustomizationStruct actual = LinqToLcs.GetLcs(queryExpression, Utils.GetDefaultView(typeof(Лапа)));

            var expected = new LoadingCustomizationStruct(null)
            {
                LimitFunction =
                    this.ldef.GetFunction(
                        this.ldef.funcL,
                        new VariableDef(this.ldef.NumericType, "РазмерNullableInt"),
                        size)
            };
            Assert.True(Equals(expected, actual));
        }
    }
}
