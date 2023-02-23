namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NewPlatform.Flexberry.ORM.IntegratedTests;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// Класс для тестирования динамических представлений при работе с LINQProvider'ом.
    /// </summary>
    public class TestDynamicView : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public TestDynamicView() : base("LTest2")
        {
        }

        /// <summary>
        /// Метод проверки наложения ограничения на PK мастера, добавленного в динамическое представление.
        /// </summary>
        [Fact]
        public void TestDynamicViewMaster()
        {
            foreach (var dataService in DataServices)
            {
                View view = new View { DefineClassType = typeof(Кошка) };

                view.AddProperty(Information.ExtractPropertyPath<Кошка>(x => x.Кличка));
                view.AddProperty(Information.ExtractPropertyPath<Кошка>(x => x.Порода));
                view.AddProperty(Information.ExtractPropertyPath<Кошка>(x => x.Порода.Название));

                // Кличка и PK породы не имеют значения, т.к. мы не проверяем вычитку из базы.
                var query = ((SQLDataService)dataService)
                    .Query<Кошка>(view)
                    .Where(x => x.Кличка == "Тузик" && x.Порода.__PrimaryKey.Equals("CD6058FF-0426-4C6E-86F3-3F62F5B9ABF0"));

                Assert.Null(query.FirstOrDefault());
            }
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании булевского поля.
        /// </summary>
        [Fact]
        public void GetLcsTestBooleanVariableDefWithDynamicView()
        {
            var testProvider = new TestLcsQueryProvider<ТипЛапы>();
            new Query<ТипЛапы>(testProvider).Where(o => o.Актуально).ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(ТипЛапы));
            Assert.NotNull(actual.View);
            Assert.Equal(1, actual.View.Properties.Count());
            Assert.Equal(Information.ExtractPropertyPath<ТипЛапы>(x => x.Актуально), actual.View.Properties[0].Name);
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля типа дата.
        /// </summary>
        [Fact]
        public void GetLcsTestDateTimeNowWithDynamicView()
        {
            var testProvider = new TestLcsQueryProvider<Перелом>();
            new Query<Перелом>(testProvider).Where(o => o.Дата <= DateTime.Now).ToArray();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Перелом));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Перелом>(x => x.Дата)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля типа KeyGuid.
        /// </summary>
        [Fact]
        public void TestUseGuidPropertyWithDynamicView()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => o.Ключ == "{72FCA622-A01E-494C-BE1C-0821178594FB}").ToArray();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Кошка));

            Assert.NotNull(actual.View);
            Assert.Equal(1, actual.View.Properties.Count());
            Assert.Equal(Information.ExtractPropertyPath<Кошка>(x => x.Ключ), actual.View.Properties[0].Name);
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании мастерового поля.
        /// </summary>
        [Fact]
        public void GetLcsTestDataObjectWithDynamicView()
        {
            var порода = new Порода();
            var testProvider = new TestLcsQueryProvider<Кошка>();

            new Query<Кошка>(testProvider).Where(o => порода.Equals(o.Порода)).ToArray();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Кошка));

            Assert.NotNull(actual.View);
            Assert.Equal(1, actual.View.Properties.Count());
            Assert.Equal(Information.ExtractPropertyPath<Кошка>(x => x.Порода), actual.View.Properties[0].Name);
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля мастера.
        /// </summary>
        [Fact]
        public void GetLcsTestDataObjectWithDynamicView2()
        {
            var порода = new Порода { Название = "тест" };
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(o => o.Порода.Название.Equals(порода.Название)).ToArray();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Кошка));
            Assert.NotNull(actual.View);
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Кошка>(x => x.Порода)));
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Кошка>(x => x.Порода.Название)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля целого типа.
        /// </summary>
        [Fact]
        public void GetLcsTestIntParseWithDynamicView()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(o => o.Номер == 1).ToArray();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.Номер)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля дробного типа.
        /// </summary>
        [Fact]
        public void GetLcsTestDoubleWithDynamicView()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерDouble < 43.23423).ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.РазмерDouble)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля дробного типа.
        /// </summary>
        [Fact]
        public void GetLcsTestFloatWithDynamicView()
        {
            const float Size = 43.76F;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерFloat < Size).ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.РазмерFloat)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля Nullable-целого типа.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableIntWithDynamicView()
        {
            int? size = 4;
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableInt < size).ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.РазмерNullableInt)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля дробного типа.
        /// </summary>
        [Fact]
        public void GetLcsTestDecimalWithDynamicView()
        {
            const decimal Size = (decimal)43.23423;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерDecimal < Size).ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.РазмерDecimal)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля Nullable-дробного типа.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableDecimalWithDynamicView()
        {
            int? size = 4;

            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableDecimal < size).ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.РазмерNullableDecimal)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля строкового типа.
        /// </summary>
        [Fact]
        public void GetLcsTestStringEqualsWithDynamicView()
        {
            var query = from pn in new List<Кошка>().AsQueryable() where pn.Кличка.Equals("Кошка") select pn;
            var queryExpression = query.Expression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Кошка));

            Assert.NotNull(actual.View);
            Assert.Equal(1, actual.View.Properties.Count());
            Assert.Equal(Information.ExtractPropertyPath<Кошка>(x => x.Кличка), actual.View.Properties[0].Name);
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля символьного типа.
        /// </summary>
        [Fact]
        public void GetLcsTestCharWithDynamicView()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерChar == 'f').ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.РазмерChar)));
        }

        /// <summary>
        /// Проверяем формирование динамического представления при использовании поля Nullable-символьного типа.
        /// </summary>
        [Fact]
        public void GetLcsTestNullableCharWithDynamicView()
        {
            var testProvider = new TestLcsQueryProvider<Лапа>();
            new Query<Лапа>(testProvider).Where(x => x.РазмерNullableChar == null).ToList();
            var queryExpression = testProvider.InnerExpression;
            var actual = LinqToLcs.GetLcs(queryExpression, typeof(Лапа));

            Assert.NotNull(actual.View);

            // Свойства 2, поскольку если есть агрегирующее свойство, то оно обязательно включается.
            Assert.Equal(2, actual.View.Properties.Count());
            Assert.True(actual.View.Properties.Select(x => x.Name).Contains(Information.ExtractPropertyPath<Лапа>(x => x.РазмерNullableChar)));
        }
    }
}