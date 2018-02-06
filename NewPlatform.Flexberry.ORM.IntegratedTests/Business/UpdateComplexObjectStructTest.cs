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

        /// <summary>
        /// Тест обновления свойства мастера, которое не было вычитано.
        /// </summary>
        [Fact]
        public void UpdateMasterPropertyTest()
        {
            foreach (IDataService dataService in DataServices)
            {
                // Arrange.
                string названиеЛеса = "Шишкин лес";
                string новоеНазваниеЛеса = "Лисицын лес";
                var лес = new Лес {Название = названиеЛеса};
                var потапыч = new Медведь {ПорядковыйНомер = 5, ЛесОбитания = лес};
                dataService.UpdateObject(потапыч);

                View view = new View
                {
                    DefineClassType = typeof(Медведь),
                    Properties = new[]
                    {
                        new PropertyInView(nameof(Медведь.ЛесОбитания), string.Empty, true, string.Empty)
                    }
                };

                LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view);
                var dataObjects = dataService.LoadObjects(lcs);

                Assert.Equal(1, dataObjects.Length);

                Медведь медведь = (Медведь)dataObjects[0];

                // Assert.AreEqual(названиеЛеса, медведь.ЛесОбитания.Название);

                // Act.
                медведь.ЛесОбитания.Название = новоеНазваниеЛеса;
                dataService.UpdateObject(медведь.ЛесОбитания);

                // Assert.
                view.AddProperty(Information.ExtractPropertyPath<Медведь>(м => м.ЛесОбитания.Название));
                dataObjects = dataService.LoadObjects(lcs);

                Assert.Equal(1, dataObjects.Length);

                медведь = (Медведь)dataObjects[0];
                Assert.Equal(названиеЛеса, медведь.ЛесОбитания.Название);
            }
        }
    }
}
