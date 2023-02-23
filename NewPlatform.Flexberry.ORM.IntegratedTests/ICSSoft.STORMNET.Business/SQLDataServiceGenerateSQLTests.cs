namespace NewPlatform.Flexberry.ORM.IntegratedTests.Business
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    using NewPlatform.Flexberry.ORM.Tests;

    using Xunit;

    public class SQLDataServiceGenerateSQLTests : BaseIntegratedTest
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SQLDataServiceGenerateSQLTests()
            : base("SQLDSG")
        {
        }

        /// <summary>
        /// Тест проверяет метод <see cref="SQLDataService.GenerateSQLSelect(LoadingCustomizationStruct,bool)"/>,
        /// чтобы в sql-запрос не добавляется left join, когда от мастера требуется только его первичный ключ.
        /// </summary>
        [Fact]
        public void GenerateSQLSelectNoJoinsTest()
        {
            foreach (SQLDataService ds in DataServices)
            {
                // Arrange.
                var view0 = new View { DefineClassType = typeof(Медведь) };
                view0.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания));
                var lcs0 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view0);

                var view1 = new View { DefineClassType = typeof(Медведь) };
                view1.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey));
                var lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view1);

                var view2 = new View { DefineClassType = typeof(Медведь) };
                view2.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey));
                var lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view2);

                var view3 = new View { DefineClassType = typeof(Медведь) };
                view3.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.__PrimaryKey),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания));
                var lcs3 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view3);

                // Act.
                string query0 = ds.GenerateSQLSelect(lcs0, false);
                string query1 = ds.GenerateSQLSelect(lcs1, false);
                string query2 = ds.GenerateSQLSelect(lcs2, false);
                string query3 = ds.GenerateSQLSelect(lcs3, false);

                // Assert.
                ds.ExecuteNonQuery(query0);
                ds.ExecuteNonQuery(query1);
                ds.ExecuteNonQuery(query2);
                ds.ExecuteNonQuery(query3);
            }
        }

        /// <summary>
        /// Тест проверяет метод <see cref="SQLDataService.GenerateSQLSelect(LoadingCustomizationStruct,bool)"/>,
        /// чтобы в sql-запрос добавляется только один необходимый left join.
        /// </summary>
        [Fact]
        public void GenerateSQLSelectSingleJoinTest()
        {
            foreach (SQLDataService ds in DataServices)
            {
                // Arrange.
                var view0 = new View { DefineClassType = typeof(Медведь) };
                view0.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна));
                var lcs0 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view0);

                var view1 = new View { DefineClassType = typeof(Медведь) };
                view1.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна));
                var lcs1 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view1);

                var view2 = new View { DefineClassType = typeof(Медведь) };
                view2.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна.__PrimaryKey));
                var lcs2 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view2);

                var view3 = new View { DefineClassType = typeof(Медведь) };
                view3.AddProperties(
                    Information.ExtractPropertyPath<Медведь>(x => x.Вес),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна.__PrimaryKey),
                    Information.ExtractPropertyPath<Медведь>(x => x.ЛесОбитания.Страна));
                var lcs3 = LoadingCustomizationStruct.GetSimpleStruct(typeof(Медведь), view3);

                // Act.
                string query0 = ds.GenerateSQLSelect(lcs0, false);
                string query1 = ds.GenerateSQLSelect(lcs1, false);
                string query2 = ds.GenerateSQLSelect(lcs2, false);
                string query3 = ds.GenerateSQLSelect(lcs3, false);

                // Assert.
                ds.ExecuteNonQuery(query0);
                ds.ExecuteNonQuery(query1);
                ds.ExecuteNonQuery(query2);
                ds.ExecuteNonQuery(query3);

                Assert.DoesNotContain(query0, "LEFT JOIN");
                Assert.DoesNotContain(query1, "LEFT JOIN");
                Assert.DoesNotContain(query2, "LEFT JOIN");
                Assert.DoesNotContain(query3, "LEFT JOIN");
            }
        }
    }
}
