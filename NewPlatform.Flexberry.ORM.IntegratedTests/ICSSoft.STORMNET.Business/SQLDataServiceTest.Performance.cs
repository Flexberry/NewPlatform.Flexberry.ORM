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

        /// <summary>
        /// Тест работы логики для корректной обработки интерфейса <see cref="ICSSoft.STORMNET.Business.Interfaces.IReferencesNullDelete" />.
        /// </summary>
        [Fact(Skip = "Для ручного тестирования")]
        public void DeleteWithIReferencesNullDeletePerformanceTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                const int length = 1000;
                IEnumerable<int> range = Enumerable.Range(1, length);

                // Объекты с интерфейсом IReferencesNullDelete, но без ссылок на них.
                DataObject[] objsWithIRefNoRef = range.Select(
                    x => new Plant2
                    {
                        Name = "Объекты с интерфейсом IReferencesNullDelete, но без ссылок на них.",
                    }).ToArray();
                dataService.UpdateObjects(ref objsWithIRefNoRef);

                // Объекты с интерфейсом IReferencesNullDelete, со ссылками на них.
                DataObject[] objsWithIRefAndRef = range.Select(
                    x => new Plant2
                    {
                        Name = "Объекты с интерфейсом IReferencesNullDelete, со ссылками на них.",
                    }).ToArray();
                dataService.UpdateObjects(ref objsWithIRefAndRef);

                DataObject[] objsRefs = range.Select(
                    x => new Dish2
                    {
                        DishName = "Объекты со ссылками.",
                        MainIngridient = (Plant2)objsWithIRefAndRef[x - 1],
                    }).ToArray();
                dataService.UpdateObjects(ref objsRefs);

                // Объекты без интерфейса IReferencesNullDelete.
                DataObject[] objsWithoutIRef = range.Select(
                    x => new Dish2
                    {
                        DishName = "Объекты без интерфейса IReferencesNullDelete.",
                    }).ToArray();
                dataService.UpdateObjects(ref objsWithoutIRef);

                // Объекты с интерфейсом IReferencesNullDelete и иерархией.
                DataObject[] objsWithIRefAndSelfRef = range.Select(
                    x => new HierarchyClassWithIRND
                    {
                        Name = "Объекты с интерфейсом IReferencesNullDelete и иерархией.",
                    }).ToArray();
                dataService.UpdateObjects(ref objsWithIRefAndSelfRef);

                DataObject[] objsWithIRefAndSelfRef2 = range.Select(
                    x => new HierarchyClassWithIRND
                    {
                        Name = "Объекты с интерфейсом IReferencesNullDelete и иерархией.",
                        Parent = (HierarchyClassWithIRND)objsWithIRefAndSelfRef[x - 1],
                    }).ToArray();
                dataService.UpdateObjects(ref objsWithIRefAndSelfRef2);

                DataObject[] objsWithIRefAndSelfRef3 = range.Select(
                    x => new HierarchyClassWithIRND
                    {
                        Name = "Объекты с интерфейсом IReferencesNullDelete и иерархией.",
                    }).ToArray();
                dataService.UpdateObjects(ref objsWithIRefAndSelfRef3);

                // Простановка статуса на удаление.
                for (int i = 0; i < length; i++)
                {
                    objsWithIRefNoRef[i].SetStatus(ObjectStatus.Deleted);
                    objsWithIRefAndRef[i].SetStatus(ObjectStatus.Deleted);
                    objsWithoutIRef[i].SetStatus(ObjectStatus.Deleted);
                    objsWithIRefAndSelfRef[i].SetStatus(ObjectStatus.Deleted);
                    objsWithIRefAndSelfRef3[i].SetStatus(ObjectStatus.Deleted);
                }

                // Act.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objsWithIRefNoRef);
                stopwatch.Stop();
                var objsWithIRefNoRefTime = stopwatch.ElapsedMilliseconds;

                stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objsWithIRefAndRef);
                stopwatch.Stop();
                var objsWithIRefAndRefTime = stopwatch.ElapsedMilliseconds;

                stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objsWithoutIRef);
                stopwatch.Stop();
                var objsWithoutIRefTime = stopwatch.ElapsedMilliseconds;

                stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objsWithIRefAndSelfRef);
                stopwatch.Stop();
                var objsWithIRefAndSelfRefTime = stopwatch.ElapsedMilliseconds;

                stopwatch = new Stopwatch();
                stopwatch.Start();
                dataService.UpdateObjects(ref objsWithIRefAndSelfRef3);
                stopwatch.Stop();
                var objsWithIRefAndSelfRef3Time = stopwatch.ElapsedMilliseconds;

                // В настоящий момент объекты без интерфейса удаляются значительно быстрее, потому что в БС интерфейса для каждого удаляемого объекта осуществляются запросы в БД ко всем типам, которые могут иметь на него ссылку.
                output.WriteLine($"{nameof(DeleteWithIReferencesNullDeletePerformanceTest)}@{dataService.GetType().Name}: with IRef and no refs: elapsed {objsWithIRefNoRefTime}");
                output.WriteLine($"{nameof(DeleteWithIReferencesNullDeletePerformanceTest)}@{dataService.GetType().Name}: with IRef and refs: elapsed {objsWithIRefAndRefTime}");
                output.WriteLine($"{nameof(DeleteWithIReferencesNullDeletePerformanceTest)}@{dataService.GetType().Name}: no IRef: elapsed {objsWithoutIRefTime}");
                output.WriteLine($"{nameof(DeleteWithIReferencesNullDeletePerformanceTest)}@{dataService.GetType().Name}: with IRef and self ref: elapsed {objsWithIRefAndSelfRefTime}");
                output.WriteLine($"{nameof(DeleteWithIReferencesNullDeletePerformanceTest)}@{dataService.GetType().Name}: with IRef and empty self ref: elapsed {objsWithIRefAndSelfRef3Time}");
            }
        }
    }
}
