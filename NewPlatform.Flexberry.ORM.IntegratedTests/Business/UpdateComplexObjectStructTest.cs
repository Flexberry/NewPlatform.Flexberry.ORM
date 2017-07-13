namespace ICSSoft.STORMNET.Tests.TestClasses.Business
{
    using ICSSoft.STORMNET.KeyGen;
    using Xunit;
    using NewPlatform.Flexberry.ORM.Tests;
    using ICSSoft.STORMNET.Business;
    using System.Configuration;
    using NewPlatform.Flexberry.ORM.IntegratedTests;

    /// <summary>
    /// Класс для проверки методов обновления объектов данных в сложной структуре.
    /// </summary>

    public class UpdateComplexObjectStructTest : BaseIntegratedTest
    {
        // <summary>
        /// Конструктор.
        /// </summary>
        public UpdateComplexObjectStructTest()
            : base("CompObj")
        {
        }

        /// <summary>
        /// Шаг 1. Создание экземпляра класса А хотя бы с одним детейлом класса В.
        /// Шаг 2. Создание другого экземпляра класса А с установкой первичного ключа первого экземпляра.
        /// Шаг 3. Зачитка второго экземпляра класса А по представлению, не содержащему детейлы класса В.
        /// Шаг 4. Создание экземпляра класса С с установкой в качестве мастера второго экземпляра класса А.
        /// Шаг 5. Обновление экземпляра класса С в базу данных.
        /// </summary>
        [Fact]
        public void UpdateOtherAgregatorInstanceTest()
        {
            foreach (IDataService dataService in DataServices)
            {

                // Шаг 1. Создание экземпляра класса А хотя бы с одним детейлом класса В.
                var берлога = new Берлога {Наименование = "У потапыча"};
                var потапыч = new Медведь {ПорядковыйНомер = 5};
                потапыч.Берлога.Add(берлога);
                dataService.UpdateObject(потапыч);

                // Шаг 2. Создание другого экземпляра класса А с установкой первичного ключа первого экземпляра.
                var pk = (KeyGuid) потапыч.__PrimaryKey;
                var берёзыч = new Медведь {ПорядковыйНомер = 6};
                берёзыч.SetExistObjectPrimaryKey(pk);

                // Шаг 3. Допустимо пропустить.

                // Шаг 4. Создание экземпляра класса С с установкой в качестве мастера второго экземпляра класса А.
                var блоха = new Блоха {Кличка = "Попрыгушка", МедведьОбитания = берёзыч};

                // Шаг 5. Обновление экземпляра класса С в базу данных.
                dataService.UpdateObject(блоха);

                Assert.Equal(1, потапыч.Берлога.Count);
            }
        }
    }
}
