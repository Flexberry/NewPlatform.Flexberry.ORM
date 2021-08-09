namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    /// <summary>
    /// Тесты производительности <see cref="SQLDataService" />.
    /// </summary>
    public partial class SQLDataServiceTest
    {
        /// <summary>
        /// Тест удаления детейлов.
        /// </summary>
        [Fact(Skip = "Для ручного тестирования")]
        public void DeleteNotLoadedAgregatorWithDetailsTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                const int length = 1000;
                IEnumerable<int> range = Enumerable.Range(0, length);
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
