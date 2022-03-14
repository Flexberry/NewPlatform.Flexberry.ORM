namespace NewPlatform.Flexberry.ORM.IntegratedTests
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
    using Xunit;
    using Xunit.Abstractions;
    using ICSSoft.STORMNET.UserDataTypes;
    using NewPlatform.Flexberry.ORM.Tests;

    /// <summary>
    /// Тесты асинхронных методов.
    /// </summary>
    public class DataServiceAsyncTests : BaseIntegratedTest
    {
        /// <inheritdoc cref="BaseSQLDataServiceTests" />
        public DataServiceAsyncTests()
            : base("async") { }

        /// <summary>
        /// Тест проверяет метод <see cref="IAsyncDataService.GetObjectsCountAsync(LoadingCustomizationStruct)"/>.
        /// </summary>
        [Fact]
        public async void GetObjectsCountAsyncTest()
        {
            foreach (SQLDataService ds in DataServices)
            {
                // Arrange.
                var user1 = new Пользователь { ФИО = "Петриченко Максим Андреевич", Логин = "petr", ДатаРегистрации = NullableDateTime.Parse("2020-01-01") };
                var user2 = new Пользователь { ФИО = "Катаев Владимир Владимирович", Логин = "vlad", ДатаРегистрации = NullableDateTime.Parse("2021-10-28") };
                var user3 = new Пользователь { ФИО = "Кутузов Максим Юрьевич", Логин = "max", ДатаРегистрации = NullableDateTime.Parse("1999-10-10") };
                var user4 = new Пользователь { ФИО = "Поклонский Андрей Иванович", Логин = "andrew", ДатаРегистрации = NullableDateTime.Parse("1995-03-01") };
                var dObjects = new ICSSoft.STORMNET.DataObject[] { user1, user2, user3, user4 };

                var view = new View { DefineClassType = typeof(Пользователь) };
                view.AddProperties(
                    Information.ExtractPropertyPath<Пользователь>(x => x.ФИО),
                    Information.ExtractPropertyPath<Пользователь>(x => x.Логин),
                    Information.ExtractPropertyPath<Пользователь>(x => x.ДатаРегистрации));

                var lcsDateAfter2000 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Пользователь), view);
                lcsDateAfter2000.LimitFunction = FunctionBuilder.BuildGreaterOrEqual<Пользователь>(x => x.ДатаРегистрации, new System.DateTime(2020, 01, 01));

                // Act.
                ds.UpdateObjects(ref dObjects);
                var count = await ds.GetObjectsCountAsync(lcsDateAfter2000);

                // Assert.
                Assert.Equal(2, count);
            }
        }

        /// <summary>
        /// Тест проверяет метод <see cref="IAsyncDataService.LoadObjectAsync(ICSSoft.STORMNET.DataObject, View, bool, bool)"/>.
        /// </summary>
        [Fact]
        public async void LoadObjectAsyncTest()
        {
            foreach (SQLDataService ds in DataServices)
            {
                // Arrange.
                var user1 = new Пользователь { ФИО = "Петриченко Максим Андреевич", Логин = "petr", ДатаРегистрации = NullableDateTime.Parse("2020-01-01") };
                var dObjects = new ICSSoft.STORMNET.DataObject[] { user1 };

                var view = new View { DefineClassType = typeof(Пользователь) };
                view.AddProperties(
                    Information.ExtractPropertyPath<Пользователь>(x => x.ФИО),
                    Information.ExtractPropertyPath<Пользователь>(x => x.Логин),
                    Information.ExtractPropertyPath<Пользователь>(x => x.ДатаРегистрации));

                // Act.
                ds.UpdateObjects(ref dObjects);

                var userToLoad = new Пользователь();
                userToLoad.SetExistObjectPrimaryKey(user1.__PrimaryKey);
                await ds.LoadObjectAsync(view, userToLoad);

                // Assert.
                Assert.Equal(user1.ФИО, userToLoad.ФИО);
                Assert.Equal(user1.Логин, userToLoad.Логин);
                Assert.Equal(user1.ДатаРегистрации, userToLoad.ДатаРегистрации);
            }
        }
    }
}
