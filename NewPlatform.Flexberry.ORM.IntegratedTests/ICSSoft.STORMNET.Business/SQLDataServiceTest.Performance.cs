namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты производительности <see cref="SQLDataService" />.
    /// </summary>
    public partial class SQLDataServiceTest
    {
        private const int TestObjectCount = 1000;

        /// <summary>
        /// Тест создания объектов без детейловых записей.
        /// </summary>
        [Fact(Skip = "Для ручного тестирования")]
        public void CreateAgregatorWithoutDetailsTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                IEnumerable<int> range = Enumerable.Range(0, TestObjectCount);
                var author = new Пользователь
                {
                    Логин = "BrainStorm",
                };
                dataService.UpdateObject(author);
                var contest = new Конкурс
                {
                    Название = "Мозговой штурм",
                    Организатор = author,
                };
                dataService.UpdateObject(contest);

                // Act.
                DataObject[] objs = range.Select(
                    x => new Идея
                    {
                        СуммаБаллов = x,
                        Автор = author,
                        Конкурс = contest,
                    }).ToArray();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objs);
                stopwatch.Stop();

                output.WriteLine($"{nameof(CreateAgregatorWithoutDetailsTest)}@{dataService.GetType().Name}: elapsed {stopwatch.ElapsedMilliseconds}");
            }
        }

        /// <summary>
        /// Тест чтения объектов без детейловых записей.
        /// </summary>
        [Fact(Skip = "Для ручного тестирования")]
        public void ReadAgregatorWithoutDetailsTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                IEnumerable<int> range = Enumerable.Range(0, TestObjectCount);
                var author = new Пользователь
                {
                    Логин = "BrainStorm",
                };
                dataService.UpdateObject(author);
                var contest = new Конкурс
                {
                    Название = "Мозговой штурм",
                    Организатор = author,
                };
                dataService.UpdateObject(contest);

                // Генерируем много объектов с детейлами.
                DataObject[] objs = range.Select(
                    x => new Идея
                    {
                        СуммаБаллов = x,
                        Автор = author,
                        Конкурс = contest,
                    }).ToArray();
                dataService.UpdateObjects(ref objs);

                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Идея), Идея.Views.ИдеяE);
                lcs.LimitFunction = FunctionBuilder.BuildAnd(
                    FunctionBuilder.BuildEquals<Идея>(x => x.Автор, author),
                    FunctionBuilder.BuildEquals<Идея>(x => x.Конкурс, contest));

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var loadedObjs = dataService.LoadObjects(lcs);
                stopwatch.Stop();

                output.WriteLine($"{nameof(ReadAgregatorWithoutDetailsTest)}@{dataService.GetType().Name}: elapsed {stopwatch.ElapsedMilliseconds}");
            }
        }

        /// <summary>
        /// Тест удаления объектов с детейлами, но без детейловых записей.
        /// </summary>
        [Fact(Skip = "Для ручного тестирования")]
        public void DeleteNotLoadedAgregatorWithDetailsTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                IEnumerable<int> range = Enumerable.Range(0, TestObjectCount);
                var author = new Пользователь
                {
                    Логин = "BrainStorm",
                };
                dataService.UpdateObject(author);
                var contest = new Конкурс
                {
                    Название = "Мозговой штурм",
                    Организатор = author,
                };
                dataService.UpdateObject(contest);

                // Генерируем много объектов с детейлами.
                DataObject[] objs = range.Select(
                    x => new Идея
                    {
                        СуммаБаллов = x,
                        Автор = author,
                        Конкурс = contest,
                    }).ToArray();
                dataService.UpdateObjects(ref objs);

                // Act.
                // Удаляем все объекты.
                DataObject[] objsForDelete = objs.Select(
                    x =>
                    {
                        var obj = PKHelper.CreateDataObject<Идея>(x);
                        obj.SetStatus(ObjectStatus.Deleted);
                        return obj;
                    }).ToArray();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objsForDelete);
                stopwatch.Stop();

                output.WriteLine($"{nameof(DeleteNotLoadedAgregatorWithDetailsTest)}@{dataService.GetType().Name}: elapsed {stopwatch.ElapsedMilliseconds}");
            }
        }
    }
}
