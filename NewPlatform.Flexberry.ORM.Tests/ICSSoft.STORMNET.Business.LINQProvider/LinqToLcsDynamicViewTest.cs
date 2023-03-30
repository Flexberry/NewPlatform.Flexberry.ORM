namespace ICSSoft.STORMNET.Business.LINQProvider.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ICSSoft.STORMNET.Windows.Forms;
    using NewPlatform.Flexberry.ORM.Tests;
    using Xunit;

    /// <summary>
    /// В методах этого класса проверяется только формирование представлений, но не выражений.
    /// </summary>
    public class LinqToLcsDynamicViewTest
    {
        private static readonly string КличкаName = Information.ExtractPropertyName<Кошка>(x => x.Кличка);
        private static readonly string ПородаName = Information.ExtractPropertyName<Кошка>(x => x.Порода);
        private static readonly string ЛапаName = Information.ExtractPropertyName<Кошка>(x => x.Лапа);
        private static readonly string РазмерName = Information.ExtractPropertyName<Лапа>(x => x.Размер);
        private static readonly string ПереломName = Information.ExtractPropertyName<Лапа>(x => x.Перелом);
        private static readonly string ТипName = Information.ExtractPropertyName<Перелом>(x => x.Тип);

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём некооректное имя свойства.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewUncorrectPropertyName()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.AddPropertyToView(new View(), null, false);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём некооректное имя свойства.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewUncorrectPropertyName2()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                UtilsLcs.AddPropertyToView(new View(), string.Empty, false);
            });
            Assert.IsType(typeof(ArgumentNullException), exception);
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём существующие свойства в нединамическое представление.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewNotDinamicView()
        {
            View view = Utils.GetDefaultView(typeof(Кошка));
            UtilsLcs.AddPropertyToView(view, КличкаName, false);
            UtilsLcs.AddPropertyToView(view, "Порода.ТипПороды", false);
            Assert.Equal(Utils.GetDefaultView(typeof(Кошка)).Properties.Count(), view.Properties.Count());
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём несуществующие свойства в нединамическое представление.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewNotDinamicViewWithError()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                View view = Utils.GetDefaultView(typeof(Кошка));
                UtilsLcs.AddPropertyToView(view, "НесуществующееСвойство", false);
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём несуществующие свойства мастера в нединамическое представление.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewNotDinamicViewWithError2()
        {
            var exception = Xunit.Record.Exception(() =>
            {
                View view = Utils.GetDefaultView(typeof(Кошка));
                UtilsLcs.AddPropertyToView(view, "Порода.НесуществующееСвойство", false);
            });
            Assert.IsType(typeof(ArgumentException), exception);
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём несуществующие свойства в динамическое представление.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewDinamicView()
        {
            View view = Utils.GetDefaultView(typeof(Кошка));
            UtilsLcs.AddPropertyToView(view, "НесуществующееСвойство", true);
            Assert.Equal(Utils.GetDefaultView(typeof(Кошка)).Properties.Count() + 1, view.Properties.Count());
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём несуществующие свойства мастера в динамическое представление.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewDinamicView2()
        {
            View view = Utils.GetDefaultView(typeof(Кошка));
            UtilsLcs.AddPropertyToView(view, "Порода.НесуществующееСвойство", true);
            Assert.Equal(Utils.GetDefaultView(typeof(Кошка)).Properties.Count() + 1, view.Properties.Count());
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Передаём несуществующие свойства несуществующего мастера в динамическое представление.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewDinamicView3()
        {
            View view = Utils.GetDefaultView(typeof(Кошка));
            UtilsLcs.AddPropertyToView(view, "НесуществующееСвойство.НесуществующееСвойство", true);
            Assert.Equal(Utils.GetDefaultView(typeof(Кошка)).Properties.Count() + 2, view.Properties.Count());
        }

        /// <summary>
        /// Проверка добавления свойства в представление. Если есть агрегатор, то он автоматически должен быть добавлен в динамическое представление.
        /// </summary>
        [Fact]
        public void TestAddPropertyToViewDinamicViewAndAgregetor()
        {
            View view = Utils.GetDefaultView(typeof(Перелом));
            Assert.False(view.Properties.Where(x => x.Name == ЛапаName).Any());
            UtilsLcs.AddPropertyToView(view, "НесуществующееСвойство", true);
            Assert.Equal(Utils.GetDefaultView(typeof(Перелом)).Properties.Count() + 2, view.Properties.Count());
            Assert.True(view.Properties.Where(x => x.Name == ЛапаName).Any());
        }

        /// <summary>
        /// Самое простое ограничение.
        /// </summary>
        [Fact]
        public void LcsDynamicViewTestForSimpleFunction()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(k => k.Кличка == "кошка").ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var result = LinqToLcs.GetLcs<Кошка>(queryExpression);

            // Проверка, что использованное свойство есть в представлении
            Assert.True(result.View.CheckPropname(КличкаName));
        }

        /// <summary>
        /// Ограничение на детейлы.
        /// </summary>
        [Fact]
        public void LcsDynamicViewTestForDetail()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(k => k.Лапа.Cast<Лапа>().Any(l => l.Размер > 5)).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var result = LinqToLcs.GetLcs<Кошка>(queryExpression);

            Assert.True(result.View.Details.Any(d => d.Name.Equals(ЛапаName)));
            Assert.True(result.View.GetDetail(ЛапаName).View.CheckPropname(РазмерName));
        }

        /// <summary>
        /// Ограничение на детейлы второго уровня.
        /// </summary>
        [Fact]
        public void LcsDynamicViewTestForSecondaryDetail()
        {
            var testProvider = new TestLcsQueryProvider<Кошка>();
            var закрытый = ТипПерелома.Закрытый;
            new Query<Кошка>(testProvider).Where(k => k.Лапа.Cast<Лапа>().Any(l => l.Перелом.Cast<Перелом>()
                .Any(p => p.Тип == закрытый))).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var result = LinqToLcs.GetLcs<Кошка>(queryExpression);

            Assert.True(result.View.Details.Any(d => d.Name.Equals(ЛапаName)));
            Assert.True(result.View.GetDetail(ЛапаName).View.Details.Any(d => d.Name.Equals(ПереломName)));
            Assert.True(result.View.GetDetail(ЛапаName).View.GetDetail(ПереломName).View.CheckPropname(ТипName));
        }

        /// <summary>
        /// Ограничение на мастера.
        /// </summary>
        [Fact]
        public void LcsDynamicViewTestForMaster()
        {
            var strGuid = Guid.NewGuid().ToString();

            var testProvider = new TestLcsQueryProvider<Кошка>();
            new Query<Кошка>(testProvider).Where(k => k.Порода.__PrimaryKey.ToString() == strGuid).ToList();
            Expression queryExpression = testProvider.InnerExpression;

            var result = LinqToLcs.GetLcs<Кошка>(queryExpression);

            Assert.True(result.View.Properties.Any(m => m.Name.Equals(ПородаName)));
        }

        #region Тестирование получение представления, возможно динамического.

        /// <summary>
        /// Тестируем метод <see cref="DetailVariableDef.GetPossibleDynamicView"/>.
        /// Передаём имя существующего представления.
        /// </summary>
        [Fact]
        public void GetViewMayByDynamic()
        {
            Assert.NotNull(DetailVariableDef.GetPossibleDynamicView("КошкаL", typeof(Кошка)));
        }

        /// <summary>
        /// Тестируем метод <see cref="DetailVariableDef.GetPossibleDynamicView"/>.
        /// Передаём имя несуществующего представления, при этом в файле конфигурации не задано определение интерфейса поиска представления.
        /// <remarks>Тесты, где задано определение интерфейса поиска представления, находятся в шаблоне.</remarks>
        /// </summary>
        [Fact]
        public void GetViewMayByDynamic2()
        {
            Assert.Null(DetailVariableDef.GetPossibleDynamicView("NotExistingView", typeof(Кошка)));
        }

        #endregion Тестирование получение представления, возможно динамического.
    }
}